using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using FolderBrowserEx;
using WindowsInput;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using FolderBrowserDialog = FolderBrowserEx.FolderBrowserDialog;
using Image = System.Drawing.Image;
using ListView = System.Windows.Forms.ListView;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly string systemPath;
        private readonly string persistentJsonFile;
        private readonly string persistentFilesFolder;
        private readonly string[] extensions;
        private string mediaPlayerPath;
        private List<string> videosToPlay;
        private List<Process> openedWindows = new List<Process>();
        private int indx;
        private int cols;
        private int rows;
        private CancellationToken ct;
        private CancellationTokenSource src;

        public Form1()
        {
            
            InitializeComponent();

            systemPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            persistentFilesFolder = Path.Combine(systemPath, "files");
            persistentJsonFile = Path.Combine(persistentFilesFolder, "PersistentData.json");
            extensions = new string[] { "mp4", "mov", "wmv", "flv", "avi", "webm", "mkv" };

            if (!Directory.Exists(persistentFilesFolder))
            {
                Directory.CreateDirectory(persistentFilesFolder);
            }

            if (!File.Exists(persistentJsonFile))
            {
                File.CreateText(persistentJsonFile);
            }

            Init();
        }

        private void Init()
        {
            FolderView.ItemSelectionChanged += OnFolderSelected;
            FileView.ItemCheck += OnFileChecked;

            gridCols.SelectedItem = gridCols.Items[0];
            gridRows.SelectedItem = gridRows.Items[0];

        }
        bool EndsWithOneOf(string value, IEnumerable<string> suffixes)
        {
            return suffixes.Any(value.EndsWith);
        }

        private void OnFolderSelected(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var folderPath = e.Item.Text;
            var filesInFolder = Directory.GetFiles(folderPath);
            var videoFilesInSelectedFolder = filesInFolder.Where(s => EndsWithOneOf(s, extensions)).ToList();

            FileView.Items.Clear();
            foreach (var videoFile in videoFilesInSelectedFolder)
            {
                var lvItem = new ListViewItem()
                {
                    Text = Path.GetFileName(videoFile),
                    Tag = Path.GetDirectoryName(videoFile)
                };

                var favorites = File.ReadAllLines(Path.Combine(persistentFilesFolder, "Favorites.json"));
                foreach (var line in favorites)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    var saveObj = JsonSerializer.Deserialize<FileViewItemSaveObj>(line);
                    if (saveObj.FileName.Equals(lvItem.Text) && saveObj.FilePath.Equals(lvItem.Tag))
                    {
                        lvItem.Checked = saveObj.IsChecked;
                    }
                }


                FileView.Items.Add(lvItem);
            }
        }

        private void OnFileChecked(object sender, ItemCheckEventArgs e)
        {
            var persistentFile = Path.Combine(persistentFilesFolder, "Favorites.json");
            if (!File.Exists(persistentFile))
            {
                File.CreateText(persistentFile);
            }

            var saveItem = new FileViewItemSaveObj()
            {
                EndType = "FileItem",
                FileName = FileView.Items[e.Index].Text,
                FilePath = (string) FileView.Items[e.Index].Tag,
                IsChecked = e.NewValue == CheckState.Checked
            };

            var jsonList = new List<string>();
            if (e.NewValue == CheckState.Checked)
            {
                jsonList = File.ReadAllLines(persistentFile).ToList();

                foreach (var line in jsonList)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    var saveObj = JsonSerializer.Deserialize<FileViewItemSaveObj>(line);
                    if (saveObj.FileName.Equals(saveItem.FileName))
                    {
                        return;
                    }
                }

                jsonList.Add(JsonSerializer.Serialize(saveItem));
            }
            else
            {
                jsonList = File.ReadAllLines(persistentFile).ToList();
                string lineToRemove = string.Empty;
                foreach (var line in jsonList)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    var saveObj = JsonSerializer.Deserialize<FileViewItemSaveObj>(line);
                    if (saveObj.FileName.Equals(saveItem.FileName))
                    {
                        lineToRemove = line;
                    }
                }

                if (!string.IsNullOrEmpty(lineToRemove))
                {
                    jsonList.Remove(lineToRemove);
                }

            }

            File.WriteAllLines(persistentFile, jsonList);


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
                var lvItem = new ListViewItem
                {
                    Text = folderPath
                };

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
                    EndType = "FolderItem",
                    FolderPath = item.Text,
                    IsChecked = item.Checked
                };

                jsonList.Add(JsonSerializer.Serialize(saveItem));
            }

            var playerItem = new PlayerSaveObj()
            {
                EndType = "Player",
                FilePath = MediaPlayerTitle.Tag.ToString(),
                FileName = MediaPlayerTitle.Text
            };
            jsonList.Add(JsonSerializer.Serialize(playerItem));


            File.WriteAllLines(persistentJsonFile, jsonList);

            base.OnClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            var jsonLines = File.ReadAllLines(persistentJsonFile).ToList();

            foreach (var line in jsonLines)
            {
                var obj = JsonSerializer.Deserialize<SaveObj>(line);

                switch (obj.EndType)
                {
                    case "FolderItem":
                        {
                            var saveObj = JsonSerializer.Deserialize<FolderViewItemSaveObj>(line);

                            var lvItem = new ListViewItem()
                            {
                                Text = saveObj.FolderPath,
                                Checked = saveObj.IsChecked
                            };

                            FolderView.Items.Add(lvItem);
                            break;
                        }
                    case "Player":
                        {
                            var saveObj = JsonSerializer.Deserialize<PlayerSaveObj>(line);
                            MediaPlayerTitle.Tag = saveObj.FilePath;
                            MediaPlayerTitle.Text = saveObj.FileName;
                            break;
                        }
                }
            }

            if (FolderView.Items.Count > 0)
            {
                FolderView.Items[0].Selected = true;
            }

            base.OnLoad(e);
        }

        public class FolderViewItemSaveObj : SaveObj
        {
            public string FolderPath { get; set; }
            public bool IsChecked { get; set; }
        }

        public class FileViewItemSaveObj : SaveObj
        {
            public string FilePath { get; set; }
            public string FileName { get; set; }
            public bool IsChecked { get; set; }
        }

        public class PlayerSaveObj : SaveObj
        {
            public string FilePath { get; set; }
            public string FileName { get; set; }
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

        private void btnSelectPlayer_Click(object sender, EventArgs e)
        {
            var fileBrowser = new OpenFileDialog();
            fileBrowser.ShowDialog();
            MediaPlayerTitle.Text = fileBrowser.SafeFileName;
            MediaPlayerTitle.Tag = fileBrowser.FileName;
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {

            if (StartBtn.Text.Equals("START"))
            {
                Debug.WriteLine("before calling new method");
                mediaPlayerPath = MediaPlayerTitle.Tag.ToString();
                var favoriteVideos = File.ReadAllLines(Path.Combine(persistentFilesFolder, "Favorites.json"));

                var videos = new List<string>();
                foreach (ListViewItem checkedFolder in FolderView.CheckedItems)
                {

                    foreach (var favoriteVideo in favoriteVideos)
                    {
                        var saveObj = JsonSerializer.Deserialize<FileViewItemSaveObj>(favoriteVideo);
                        if (saveObj.FilePath.Equals(checkedFolder.Text))
                        {
                            videos.Add(Path.Combine(saveObj.FilePath, saveObj.FileName));
                        }

                    }
                }

                cols = int.Parse(gridCols.SelectedItem.ToString());
                rows = int.Parse(gridRows.SelectedItem.ToString());
                isMuted = MuteCheckBox.Checked;

                Random rand = new Random();
                videosToPlay = videos.OrderBy(_ => rand.Next()).Take(cols * rows).ToList();
                StartBtn.Text = "END";


                //Form.ActiveForm.TopMost = true;
                src = new CancellationTokenSource();
                ct = src.Token;
                Task.Run(OpenWindows, ct);
            }
            else
            {

                CancelEet();
                foreach (var proc in openedWindows)
                {
                    if (proc.HasExited) continue;
                    proc.Kill();
                }

                indx = 0;
                StartBtn.Text = "START";
            }
        }
        
        private void CancelEet()
        {
            src.Cancel();
        }
        private static List<Rectangle> GetSubRectangles(Rectangle rect, int cols, int rows)
        {
            List<Rectangle> srex = new List<Rectangle>();
            int w = rect.Width / cols;
            int h = rect.Height / rows;

            for (int c = 0; c < cols; c++)
            for (int r = 0; r < rows; r++)
                srex.Add(new Rectangle(w * c, h * r, w, h));
            return srex;
        }

        private async Task OpenWindows()
        {
            var rect = new Rectangle(0, 0, Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);

            indx = 0;
            var subs = GetSubRectangles(rect, cols, rows);

            foreach (var video in videosToPlay)
            {
                var proc = Process.Start(mediaPlayerPath, video);
                while (!proc.MainWindowTitle.Equals("PotPlayer"))
                {
                    if (ct.IsCancellationRequested)
                    {
                        proc.Kill();
                        return;
                    }

                    proc.Refresh();
                }
                
                openedWindows.Add(proc);
                SetWindowPos(proc.MainWindowHandle, IntPtr.Zero, subs[indx].X, subs[indx].Y, subs[indx].Width, subs[indx].Height, SWP_NOSENDCHANGING | SWP_NOZORDER | SWP_SHOWWINDOW | SWP_ASYNCWINDOWPOS);

                if (isMuted)
                {
                    while (!proc.MainWindowTitle.Contains(Path.GetFileName(video)))
                    {
                        proc.Refresh();
                    }

                    await Task.Delay(500);

                    SetCursorPos((int)subs[indx].X + 100, (int)subs[indx].Y + 100);

                    var inp = new InputSimulator();
                    //await Task.Delay(200);
                    //inp.Mouse.LeftButtonClick();
                    //await Task.Delay(200);
                    for (int i = 0; i < 25; i++)
                    {
                        inp.Mouse.VerticalScroll(-1);
                        Thread.Sleep(10);
                    }
                }
                indx++;
            }

        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public const int SWP_ASYNCWINDOWPOS = 0x4000;
        public const int SWP_NOSENDCHANGING = 0x0400;
        public const int SWP_NOZORDER = 0x0004;
        public const int SWP_SHOWWINDOW = 0x0040;


        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        private readonly List<ListViewItem> originalFileViewState = new List<ListViewItem>();
        private bool isMuted;

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            if (originalFileViewState.Count <= 0)
            {
                foreach (ListViewItem item in FileView.Items)
                {
                    originalFileViewState.Add((ListViewItem)item.Clone());
                }
            }

            foreach (ListViewItem fileViewItem in FileView.Items)
            {
                foreach (var origItem in originalFileViewState)
                {
                    if (origItem.Text.Equals(fileViewItem.Text))
                    {
                        origItem.Checked = fileViewItem.Checked;
                    }
                }
            }

            var searchString = SearchBox.Text.ToLowerInvariant();
            if (string.IsNullOrEmpty(searchString))
            {
                FileView.Items.Clear();
                FileView.Items.AddRange(originalFileViewState.ToArray());
                originalFileViewState.Clear();
                return;
            }

            var pattern = @"([^.]+)";
            var filesToAdd = new List<ListViewItem>();

            foreach (var item in originalFileViewState)
            {
                var fileName = item.Text.ToLowerInvariant();
                
                var truncatedFileName = Regex.Match(fileName, pattern).Value;


                if (truncatedFileName.Contains(searchString))
                {
                    filesToAdd.Add((ListViewItem)item.Clone());
                }
            }

            if (filesToAdd.Any())
            {
                NumFiles.Text = filesToAdd.Count.ToString();
                FileView.Items.Clear();
                FileView.Items.AddRange(filesToAdd.ToArray());
            }
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
            
            foreach (ListViewItem selectedItem in FileView.SelectedItems)
            {
                Process.Start("explorer.exe",
                    $"/select,{Path.Combine(selectedItem.Tag.ToString(), selectedItem.Text)}");
            }
        }
    }

    public class SaveObj
    {
        public string EndType { get; set; }
    }
}
