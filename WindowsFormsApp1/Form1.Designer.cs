namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.AddFoldersBtn = new System.Windows.Forms.Button();
            this.FolderView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.FileView = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblFavoriteVideos = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelectPlayer = new System.Windows.Forms.Button();
            this.MediaPlayerTitle = new System.Windows.Forms.TextBox();
            this.StartBtn = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Remove = new System.Windows.Forms.ToolStripMenuItem();
            this.gridCols = new System.Windows.Forms.ComboBox();
            this.gridRows = new System.Windows.Forms.ComboBox();
            this.SearchBox = new System.Windows.Forms.TextBox();
            this.SearchTitle = new System.Windows.Forms.Label();
            this.NumFiles = new System.Windows.Forms.Label();
            this.NumFilesLabel = new System.Windows.Forms.Label();
            this.MuteCheckBox = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenInFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddFoldersBtn
            // 
            this.AddFoldersBtn.Location = new System.Drawing.Point(29, 37);
            this.AddFoldersBtn.Name = "AddFoldersBtn";
            this.AddFoldersBtn.Size = new System.Drawing.Size(82, 23);
            this.AddFoldersBtn.TabIndex = 0;
            this.AddFoldersBtn.Text = "Add Folders";
            this.AddFoldersBtn.UseVisualStyleBackColor = true;
            this.AddFoldersBtn.Click += new System.EventHandler(this.AddFoldersBtn_Click);
            // 
            // FolderView
            // 
            this.FolderView.AllowColumnReorder = true;
            this.FolderView.AllowDrop = true;
            this.FolderView.BackColor = System.Drawing.SystemColors.Window;
            this.FolderView.CheckBoxes = true;
            this.FolderView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.FolderView.GridLines = true;
            this.FolderView.HideSelection = false;
            this.FolderView.LabelWrap = false;
            this.FolderView.Location = new System.Drawing.Point(29, 71);
            this.FolderView.Name = "FolderView";
            this.FolderView.Size = new System.Drawing.Size(291, 356);
            this.FolderView.SmallImageList = this.imageList1;
            this.FolderView.TabIndex = 1;
            this.FolderView.UseCompatibleStateImageBehavior = false;
            this.FolderView.View = System.Windows.Forms.View.Details;
            this.FolderView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FolderView_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Folders";
            this.columnHeader1.Width = 282;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Folder");
            this.imageList1.Images.SetKeyName(1, "Video");
            // 
            // FileView
            // 
            this.FileView.AllowColumnReorder = true;
            this.FileView.AllowDrop = true;
            this.FileView.CheckBoxes = true;
            this.FileView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.FileView.GridLines = true;
            this.FileView.HideSelection = false;
            this.FileView.LabelWrap = false;
            this.FileView.Location = new System.Drawing.Point(355, 71);
            this.FileView.Name = "FileView";
            this.FileView.Size = new System.Drawing.Size(291, 356);
            this.FileView.SmallImageList = this.imageList1;
            this.FileView.TabIndex = 2;
            this.FileView.UseCompatibleStateImageBehavior = false;
            this.FileView.View = System.Windows.Forms.View.Details;
            this.FileView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FileView_MouseClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Files";
            this.columnHeader2.Width = 288;
            // 
            // lblFavoriteVideos
            // 
            this.lblFavoriteVideos.AutoSize = true;
            this.lblFavoriteVideos.BackColor = System.Drawing.SystemColors.Info;
            this.lblFavoriteVideos.Location = new System.Drawing.Point(352, 47);
            this.lblFavoriteVideos.Name = "lblFavoriteVideos";
            this.lblFavoriteVideos.Size = new System.Drawing.Size(80, 13);
            this.lblFavoriteVideos.TabIndex = 3;
            this.lblFavoriteVideos.Text = "Favorite Videos";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(665, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Grid Style";
            // 
            // btnSelectPlayer
            // 
            this.btnSelectPlayer.Location = new System.Drawing.Point(667, 47);
            this.btnSelectPlayer.Name = "btnSelectPlayer";
            this.btnSelectPlayer.Size = new System.Drawing.Size(121, 23);
            this.btnSelectPlayer.TabIndex = 6;
            this.btnSelectPlayer.Text = "Select PotPlayer";
            this.btnSelectPlayer.UseVisualStyleBackColor = true;
            this.btnSelectPlayer.Click += new System.EventHandler(this.btnSelectPlayer_Click);
            // 
            // MediaPlayerTitle
            // 
            this.MediaPlayerTitle.Location = new System.Drawing.Point(667, 76);
            this.MediaPlayerTitle.Name = "MediaPlayerTitle";
            this.MediaPlayerTitle.Size = new System.Drawing.Size(121, 20);
            this.MediaPlayerTitle.TabIndex = 7;
            // 
            // StartBtn
            // 
            this.StartBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.StartBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.StartBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.StartBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartBtn.Location = new System.Drawing.Point(667, 286);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(121, 87);
            this.StartBtn.TabIndex = 8;
            this.StartBtn.Text = "START";
            this.StartBtn.UseVisualStyleBackColor = false;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Remove});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(118, 26);
            // 
            // Remove
            // 
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(117, 22);
            this.Remove.Text = "Remove";
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // gridCols
            // 
            this.gridCols.FormattingEnabled = true;
            this.gridCols.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.gridCols.Location = new System.Drawing.Point(667, 123);
            this.gridCols.Name = "gridCols";
            this.gridCols.Size = new System.Drawing.Size(49, 21);
            this.gridCols.TabIndex = 9;
            // 
            // gridRows
            // 
            this.gridRows.FormattingEnabled = true;
            this.gridRows.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.gridRows.Location = new System.Drawing.Point(738, 123);
            this.gridRows.Name = "gridRows";
            this.gridRows.Size = new System.Drawing.Size(50, 21);
            this.gridRows.TabIndex = 10;
            // 
            // SearchBox
            // 
            this.SearchBox.Location = new System.Drawing.Point(439, 47);
            this.SearchBox.Name = "SearchBox";
            this.SearchBox.Size = new System.Drawing.Size(207, 20);
            this.SearchBox.TabIndex = 11;
            this.SearchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);
            // 
            // SearchTitle
            // 
            this.SearchTitle.AutoSize = true;
            this.SearchTitle.Location = new System.Drawing.Point(439, 28);
            this.SearchTitle.Name = "SearchTitle";
            this.SearchTitle.Size = new System.Drawing.Size(76, 13);
            this.SearchTitle.TabIndex = 12;
            this.SearchTitle.Text = "Search Videos";
            // 
            // NumFiles
            // 
            this.NumFiles.AutoSize = true;
            this.NumFiles.Location = new System.Drawing.Point(615, 430);
            this.NumFiles.Name = "NumFiles";
            this.NumFiles.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.NumFiles.Size = new System.Drawing.Size(13, 13);
            this.NumFiles.TabIndex = 13;
            this.NumFiles.Text = "0";
            // 
            // NumFilesLabel
            // 
            this.NumFilesLabel.AutoSize = true;
            this.NumFilesLabel.Location = new System.Drawing.Point(551, 430);
            this.NumFilesLabel.Name = "NumFilesLabel";
            this.NumFilesLabel.Size = new System.Drawing.Size(67, 13);
            this.NumFilesLabel.TabIndex = 14;
            this.NumFilesLabel.Text = "Files Found :";
            // 
            // MuteCheckBox
            // 
            this.MuteCheckBox.AutoSize = true;
            this.MuteCheckBox.Location = new System.Drawing.Point(667, 263);
            this.MuteCheckBox.Name = "MuteCheckBox";
            this.MuteCheckBox.Size = new System.Drawing.Size(50, 17);
            this.MuteCheckBox.TabIndex = 15;
            this.MuteCheckBox.Text = "Mute";
            this.MuteCheckBox.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenInFolder});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(151, 26);
            // 
            // OpenInFolder
            // 
            this.OpenInFolder.Name = "OpenInFolder";
            this.OpenInFolder.Size = new System.Drawing.Size(180, 22);
            this.OpenInFolder.Text = "Open in folder";
            this.OpenInFolder.Click += new System.EventHandler(this.OpenInFolder_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(801, 461);
            this.Controls.Add(this.MuteCheckBox);
            this.Controls.Add(this.NumFilesLabel);
            this.Controls.Add(this.NumFiles);
            this.Controls.Add(this.SearchTitle);
            this.Controls.Add(this.SearchBox);
            this.Controls.Add(this.gridRows);
            this.Controls.Add(this.gridCols);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.MediaPlayerTitle);
            this.Controls.Add(this.btnSelectPlayer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblFavoriteVideos);
            this.Controls.Add(this.FileView);
            this.Controls.Add(this.FolderView);
            this.Controls.Add(this.AddFoldersBtn);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Pot Player Collage";
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddFoldersBtn;
        private System.Windows.Forms.ListView FolderView;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListView FileView;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label lblFavoriteVideos;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectPlayer;
        private System.Windows.Forms.TextBox MediaPlayerTitle;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Remove;
        private System.Windows.Forms.ComboBox gridCols;
        private System.Windows.Forms.ComboBox gridRows;
        private System.Windows.Forms.TextBox SearchBox;
        private System.Windows.Forms.Label SearchTitle;
        private System.Windows.Forms.Label NumFiles;
        private System.Windows.Forms.Label NumFilesLabel;
        private System.Windows.Forms.CheckBox MuteCheckBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem OpenInFolder;
    }
}

