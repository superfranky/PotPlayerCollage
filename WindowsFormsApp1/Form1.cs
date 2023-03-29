using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using FolderBrowserDialog = FolderBrowserEx.FolderBrowserDialog;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string systemPath;
        private string persistentJsonFile;
        private string persistentFilesFolder;
        private string favoritesFile;
        private string potPlayerPath;

        private HashSet<string> extensions;

        private int cols;
        private int rows;

        private List<string> videosToPlay = new List<string>();
        private List<Process> openedWindows = new List<Process>();

        private Rectangle screenRect;

        private CancellationToken ct;
        private CancellationTokenSource src;

        private bool isMuted;
        private bool randomUnmuted;

        public Form1()
        {
            InitializeComponent();
            Setup();
        }

        private void Setup()
        {
            systemPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            persistentFilesFolder = Path.Combine(systemPath, "files");
            persistentJsonFile = Path.Combine(persistentFilesFolder, "PersistentData.json");
            favoritesFile = Path.Combine(persistentFilesFolder, "Favorites.json");
            extensions = new HashSet<string> { ".mp4", ".mov", ".wmv", ".flv", ".avi", ".webm", ".mkv" };
            screenRect = new Rectangle(0, 0, Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);

            var key = Registry.CurrentUser.OpenSubKey(@"Software\DAUM\PotPlayer64");
            potPlayerPath = key?.GetValue("ProgramPath").ToString();

            Directory.CreateDirectory(persistentFilesFolder);
            if (!File.Exists(persistentJsonFile))
            {
                File.CreateText(persistentJsonFile);
            }
            if (!File.Exists(favoritesFile))
            {
                File.Create(favoritesFile); 
            }

            FolderView.ItemSelectionChanged += OnFolderSelected;
            FileView.ItemCheck += OnFileChecked;

            gridCols.SelectedItem = gridCols.Items[0];
            gridRows.SelectedItem = gridRows.Items[0];
        }
        bool EndsWithOneOf(string value, IEnumerable<string> suffixes)
        {
            return suffixes.Any(value.EndsWith);
        }

        private List<string> filesFromFolder = new List<string>();
        private List<List<string>> savedFolders = new List<List<string>>();
        private Dictionary<string, List<string>> folderDict = new Dictionary<string, List<string>>();
        private void OnFolderSelected(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var folderPath = e.Item.Text;
            var videosInFolder = Directory.GetFiles(folderPath).Where(s => EndsWithOneOf(s, extensions)).ToList();

            FileView.BeginUpdate();
            FileView.Items.Clear();
            foreach (var video in videosInFolder)
            {
                var fileName = Path.GetFileName(video);
                var item = new ListViewItem(fileName);

                var favorites = File.ReadAllLines(favoritesFile).ToList();
                if (favorites.Contains(fileName))
                {
                    item.Checked = true;
                }

                FileView.Items.Add(item);
            }
            FileView.EndUpdate();
        }

        private void OnFileChecked(object sender, ItemCheckEventArgs e)
        {
            
            var favorites = File.ReadAllLines(favoritesFile).ToList();

            if (e.NewValue == CheckState.Checked)
            {
                if (!favorites.Contains(FileView.Items[e.Index].Text))
                {
                    favorites.Add(FileView.Items[e.Index].Text);
                }
            }
            else
            {
                if (favorites.Contains(FileView.Items[e.Index].Text))
                {
                    favorites.Remove(FileView.Items[e.Index].Text);
                }
            }
            File.WriteAllLines(favoritesFile, favorites);

        }

        private void AddFoldersBtn_Click(object sender, EventArgs e)
        {
            var folderBrowser = new FolderBrowserDialog
            {
                AllowMultiSelect = true,
                Title = "Select the folders with your videos"
            };
            folderBrowser.ShowDialog();
            var selectedFolders = folderBrowser.SelectedFolders;

            foreach (var folderPath in selectedFolders)
            {
                var lvItem = new ListViewItem(folderPath);
                FolderView.Items.Add(lvItem);
            }
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            var jsonList = new List<string>();

            foreach (ListViewItem item in FolderView.Items)
            {
                var saveItem = new FolderViewItemSaveObj()
                {
                    FolderPath = item.Text,
                    IsChecked = item.Checked
                };

                jsonList.Add(JsonSerializer.Serialize(saveItem));
            }
            File.WriteAllLines(persistentJsonFile, jsonList);

            base.OnClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!File.Exists(persistentJsonFile))
            {
                return;
            }
            var jsonLines = File.ReadAllLines(persistentJsonFile).ToList();

            FolderView.BeginUpdate();

            foreach (var line in jsonLines)
            {
                var saveObj = JsonSerializer.Deserialize<FolderViewItemSaveObj>(line);
                var lvItem = new ListViewItem()
                {
                    Text = saveObj.FolderPath,
                    Checked = saveObj.IsChecked
                };

                FolderView.Items.Add(lvItem);
            }


            if (FolderView.Items.Count > 0)
            {
                FolderView.Items[0].Selected = true;
            }

            FolderView.EndUpdate();
            base.OnLoad(e);
        }

        public class FolderViewItemSaveObj
        {
            public string FolderPath { get; set; }
            public bool IsChecked { get; set; }
        }

        private void FolderView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                FolderView.ContextMenuStrip = contextMenuStrip1;
                FolderView.ContextMenuStrip.Show(FolderView,e.Location);
                FolderView.ContextMenuStrip = null;
            }
        }


        private void Remove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in FolderView.SelectedItems)
            {
                FolderView.Items.Remove(selectedItem);
            }
        }

        private bool btnClicked = false;
        private int videoRandom;
        private List<Rectangle> subs;

        private void StartBtn_Click(object sender, EventArgs e)
        {

            if (!btnClicked)
            {
                StartBtn.Text = "END";
                btnClicked = true;

                var foldersToVideos = new Dictionary<string, List<string>>();

                foreach (ListViewItem checkedFolder in FolderView.CheckedItems)
                {
                    var videosInFolder = Directory.GetFiles(checkedFolder.Text).Where(s => extensions.Contains(Path.GetExtension(s))).Select(Path.GetFileName).ToList();
                    foldersToVideos.Add(checkedFolder.Text, videosInFolder);
                }

                var allFavorites = File.ReadAllLines(favoritesFile).ToList();
                var videos = foldersToVideos.SelectMany(kvp => kvp.Value.Where(v => allFavorites.Contains(v)).Select(v => Path.Combine(kvp.Key, v))).ToList();

                var rand = new Random();
                for (var i = 0; i < videos.Count - 1; i++)
                {
                    var j = rand.Next(i, videos.Count);
                    (videos[i], videos[j]) = (videos[j], videos[i]);
                }

                
                videosToPlay.Clear();
                videosToPlay.AddRange(videos.Take(cols * rows));

                src = new CancellationTokenSource();
                ct = src.Token;

                videoRandom = new Random().Next(0, videosToPlay.Count);
                subs = GetSubRectangles(screenRect, cols, rows);


                _ = Task.Run(OpenWindows, ct);
            }
            else
            {
                src.Cancel();
                foreach (var proc in openedWindows)
                {
                    if (proc.HasExited) continue;
                    proc.Kill();
                }

                openedWindows.Clear();

                StartBtn.Text = "START";
                btnClicked = false;
            }
        }


        private static List<Rectangle> GetSubRectangles(Rectangle rect, int cols, int rows)
        {
            var srex = new List<Rectangle>(cols * rows);
            int xStep = rect.Width / cols;
            int yStep = rect.Height / rows;

            for (int i = 0; i < cols * rows; i++)
            {
                int x = (i % cols) * xStep;
                int y = (i / cols) * yStep;
                srex.Add(new Rectangle(x, y, xStep, yStep));
            }

            return srex;
        }

        private async Task OpenWindows()
        {
            var indx = 0;

            foreach (var video in videosToPlay)
            {
                var proc = Process.Start(potPlayerPath, video);
                while (!proc.MainWindowTitle.Equals("PotPlayer"))
                {
                    if (ct.IsCancellationRequested)
                    {
                        proc.Kill();
                        return;
                    }

                    await Task.Delay(100);
                    proc.Refresh();
                }
                
                openedWindows.Add(proc);
                SetWindowPos(proc.MainWindowHandle, IntPtr.Zero, subs[indx].X, subs[indx].Y, subs[indx].Width, subs[indx].Height, SWP_NOSENDCHANGING | SWP_NOZORDER | SWP_SHOWWINDOW | SWP_ASYNCWINDOWPOS);

                if (isMuted)
                {
                    await Task.Delay(500); // wait for PotPlayer to fully load

                    int key;
                    if (!randomUnmuted || indx != videoRandom) key = VK_DOWN;
                    else key = VK_UP;

                    SendKeyMessage(proc, key);
                }
                indx++;
            }
        }

        private void SendKeyMessage(Process proc, int key)
        {
            for (var i = 0; i < 20; i++)
            {
                PostMessage(proc.MainWindowHandle, WM_KEYDOWN, key, 0);
            }
        }

        public void PopulateFileViewWithFoundFiles(string path, string searchQuery = "")
        {
            var dinfo = new DirectoryInfo(path);
            var files = dinfo.GetFiles().Where(s => extensions.Contains(s.Extension));
            FileView.BeginUpdate();
            FileView.Items.Clear();
            var favorites = File.ReadAllLines(favoritesFile).ToList();
            foreach (var fileInfo in files)
            {
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
                    if (!fileNameWithoutExtension.ToLowerInvariant().Contains(searchQuery)) continue;
                }
                var lvItem = new ListViewItem(fileInfo.Name) {Checked = favorites.Contains(fileInfo.Name)};
                FileView.Items.Add(lvItem);
            }
            FileView.EndUpdate();
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            var searchQuery = SearchBox.Text.ToLowerInvariant();
            var directoryPath = FolderView.SelectedItems[0].Text;
            PopulateFileViewWithFoundFiles(directoryPath, searchQuery);
        }

        private void FileView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                FolderView.ContextMenuStrip = contextMenuStrip2;
                FolderView.ContextMenuStrip.Show(FileView, e.Location);
                FolderView.ContextMenuStrip = null;
            }
        }

        private void OpenInFolder_Click(object sender, EventArgs e)
        {
            var selectedFolder = FolderView.SelectedItems.Count > 0 ? FolderView.SelectedItems[0].Text : null;
            var selectedVideo = FileView.SelectedItems.Count > 0 ? FileView.SelectedItems[0].Text : null;

            if (selectedFolder == null || selectedVideo == null) return;
            var fullVideoPath = Path.Combine(selectedFolder, selectedVideo);
            Process.Start("explorer.exe", $"/select, {fullVideoPath}");
        }

        private void gridCols_SelectedIndexChanged(object sender, EventArgs e)
        {
            cols = int.Parse(gridCols.SelectedItem.ToString());
        }

        private void gridRows_SelectedIndexChanged(object sender, EventArgs e)
        {

            rows = int.Parse(gridRows.SelectedItem.ToString());
        }

        private void MuteCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            isMuted = MuteCheckBox.Checked;
        }

        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        const UInt32 WM_KEYDOWN = 0x0100;
        const int VK_DOWN =	0x28;
        private const int VK_UP = 0x26;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        public const int SWP_ASYNCWINDOWPOS = 0x4000;
        public const int SWP_NOSENDCHANGING = 0x0400;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_SHOWWINDOW = 0x0040;

        private void openInPotPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedFolder = FolderView.SelectedItems.Count > 0 ? FolderView.SelectedItems[0].Text : null;
            var selectedVideo = FileView.SelectedItems.Count > 0 ? FileView.SelectedItems[0].Text : null;

            if (selectedFolder == null || selectedVideo == null) return;
            var fullVideoPath = Path.Combine(selectedFolder, selectedVideo);
            Process.Start(potPlayerPath, fullVideoPath);
        }

        private void UnmuteCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            randomUnmuted = UnmuteCheckbox.Checked;
        }
    }

}
