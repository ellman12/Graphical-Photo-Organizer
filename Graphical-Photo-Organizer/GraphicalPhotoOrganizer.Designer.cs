namespace Graphical_Photo_Organizer;

partial class GraphicalPhotoOrganizer
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.datePicker = new System.Windows.Forms.MonthCalendar();
            this.filenameLabel = new System.Windows.Forms.Label();
            this.filenameTextBox = new System.Windows.Forms.TextBox();
            this.chooseSrcDirBtn = new System.Windows.Forms.Button();
            this.srcDirLabel = new System.Windows.Forms.Label();
            this.setupGroupBox = new System.Windows.Forms.GroupBox();
            this.beginBtn = new System.Windows.Forms.Button();
            this.chooseDestDirBtn = new System.Windows.Forms.Button();
            this.destDirLabel = new System.Windows.Forms.Label();
            this.destPathLabel = new System.Windows.Forms.Label();
            this.currentPhotoGroupBox = new System.Windows.Forms.GroupBox();
            this.dateTakenSrcLabel = new System.Windows.Forms.Label();
            this.dateTakenLabel = new System.Windows.Forms.Label();
            this.resetBtn = new System.Windows.Forms.Button();
            this.skipPhotoBtn = new System.Windows.Forms.Button();
            this.deletePhotoBtn = new System.Windows.Forms.Button();
            this.nextPhotoBtn = new System.Windows.Forms.Button();
            this.photoPreview = new System.Windows.Forms.PictureBox();
            this.setupGroupBox.SuspendLayout();
            this.currentPhotoGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.photoPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // datePicker
            // 
            this.datePicker.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.datePicker.Location = new System.Drawing.Point(6, 134);
            this.datePicker.MaxSelectionCount = 1;
            this.datePicker.Name = "datePicker";
            this.datePicker.TabIndex = 1;
            this.datePicker.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.datePicker_DateChanged);
            // 
            // filenameLabel
            // 
            this.filenameLabel.AutoSize = true;
            this.filenameLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.filenameLabel.Location = new System.Drawing.Point(6, 26);
            this.filenameLabel.Name = "filenameLabel";
            this.filenameLabel.Size = new System.Drawing.Size(90, 28);
            this.filenameLabel.TabIndex = 3;
            this.filenameLabel.Text = "Filename";
            // 
            // filenameTextBox
            // 
            this.filenameTextBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.filenameTextBox.Location = new System.Drawing.Point(6, 57);
            this.filenameTextBox.Name = "filenameTextBox";
            this.filenameTextBox.Size = new System.Drawing.Size(312, 34);
            this.filenameTextBox.TabIndex = 4;
            // 
            // chooseSrcDirBtn
            // 
            this.chooseSrcDirBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chooseSrcDirBtn.Location = new System.Drawing.Point(6, 36);
            this.chooseSrcDirBtn.Name = "chooseSrcDirBtn";
            this.chooseSrcDirBtn.Size = new System.Drawing.Size(312, 41);
            this.chooseSrcDirBtn.TabIndex = 7;
            this.chooseSrcDirBtn.Text = "Choose Source Folder";
            this.chooseSrcDirBtn.UseVisualStyleBackColor = true;
            this.chooseSrcDirBtn.Click += new System.EventHandler(this.chooseSrcDirBtn_Click);
            // 
            // srcDirLabel
            // 
            this.srcDirLabel.AutoSize = true;
            this.srcDirLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.srcDirLabel.Location = new System.Drawing.Point(6, 80);
            this.srcDirLabel.Name = "srcDirLabel";
            this.srcDirLabel.Size = new System.Drawing.Size(29, 21);
            this.srcDirLabel.TabIndex = 9;
            this.srcDirLabel.Text = "dir";
            this.srcDirLabel.Click += new System.EventHandler(this.srcDirLabel_Click);
            // 
            // setupGroupBox
            // 
            this.setupGroupBox.Controls.Add(this.beginBtn);
            this.setupGroupBox.Controls.Add(this.chooseDestDirBtn);
            this.setupGroupBox.Controls.Add(this.destDirLabel);
            this.setupGroupBox.Controls.Add(this.chooseSrcDirBtn);
            this.setupGroupBox.Controls.Add(this.srcDirLabel);
            this.setupGroupBox.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.setupGroupBox.Location = new System.Drawing.Point(12, 12);
            this.setupGroupBox.Name = "setupGroupBox";
            this.setupGroupBox.Size = new System.Drawing.Size(326, 240);
            this.setupGroupBox.TabIndex = 10;
            this.setupGroupBox.TabStop = false;
            this.setupGroupBox.Text = "Setup";
            // 
            // beginBtn
            // 
            this.beginBtn.Enabled = false;
            this.beginBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.beginBtn.Location = new System.Drawing.Point(6, 185);
            this.beginBtn.Name = "beginBtn";
            this.beginBtn.Size = new System.Drawing.Size(311, 41);
            this.beginBtn.TabIndex = 14;
            this.beginBtn.Text = "Begin Sorting";
            this.beginBtn.UseVisualStyleBackColor = true;
            this.beginBtn.Click += new System.EventHandler(this.beginBtn_Click);
            // 
            // chooseDestDirBtn
            // 
            this.chooseDestDirBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.chooseDestDirBtn.Location = new System.Drawing.Point(6, 114);
            this.chooseDestDirBtn.Name = "chooseDestDirBtn";
            this.chooseDestDirBtn.Size = new System.Drawing.Size(311, 41);
            this.chooseDestDirBtn.TabIndex = 10;
            this.chooseDestDirBtn.Text = "Choose Destination Root Folder";
            this.chooseDestDirBtn.UseVisualStyleBackColor = true;
            this.chooseDestDirBtn.Click += new System.EventHandler(this.chooseDestDirBtn_Click);
            // 
            // destDirLabel
            // 
            this.destDirLabel.AutoSize = true;
            this.destDirLabel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.destDirLabel.Location = new System.Drawing.Point(6, 155);
            this.destDirLabel.Name = "destDirLabel";
            this.destDirLabel.Size = new System.Drawing.Size(29, 21);
            this.destDirLabel.TabIndex = 12;
            this.destDirLabel.Text = "dir";
            this.destDirLabel.Click += new System.EventHandler(this.destDirLabel_Click);
            // 
            // destPathLabel
            // 
            this.destPathLabel.AutoSize = true;
            this.destPathLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.destPathLabel.Location = new System.Drawing.Point(6, 485);
            this.destPathLabel.Name = "destPathLabel";
            this.destPathLabel.Size = new System.Drawing.Size(80, 25);
            this.destPathLabel.TabIndex = 6;
            this.destPathLabel.Text = "destPath";
            // 
            // currentPhotoGroupBox
            // 
            this.currentPhotoGroupBox.Controls.Add(this.dateTakenSrcLabel);
            this.currentPhotoGroupBox.Controls.Add(this.dateTakenLabel);
            this.currentPhotoGroupBox.Controls.Add(this.resetBtn);
            this.currentPhotoGroupBox.Controls.Add(this.skipPhotoBtn);
            this.currentPhotoGroupBox.Controls.Add(this.deletePhotoBtn);
            this.currentPhotoGroupBox.Controls.Add(this.destPathLabel);
            this.currentPhotoGroupBox.Controls.Add(this.nextPhotoBtn);
            this.currentPhotoGroupBox.Controls.Add(this.filenameLabel);
            this.currentPhotoGroupBox.Controls.Add(this.filenameTextBox);
            this.currentPhotoGroupBox.Controls.Add(this.datePicker);
            this.currentPhotoGroupBox.Enabled = false;
            this.currentPhotoGroupBox.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.currentPhotoGroupBox.Location = new System.Drawing.Point(12, 258);
            this.currentPhotoGroupBox.Name = "currentPhotoGroupBox";
            this.currentPhotoGroupBox.Size = new System.Drawing.Size(326, 515);
            this.currentPhotoGroupBox.TabIndex = 11;
            this.currentPhotoGroupBox.TabStop = false;
            this.currentPhotoGroupBox.Text = "Current Photo";
            // 
            // dateTakenSrcLabel
            // 
            this.dateTakenSrcLabel.AutoSize = true;
            this.dateTakenSrcLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dateTakenSrcLabel.Location = new System.Drawing.Point(180, 97);
            this.dateTakenSrcLabel.Name = "dateTakenSrcLabel";
            this.dateTakenSrcLabel.Size = new System.Drawing.Size(36, 28);
            this.dateTakenSrcLabel.TabIndex = 19;
            this.dateTakenSrcLabel.Text = "src";
            // 
            // dateTakenLabel
            // 
            this.dateTakenLabel.AutoSize = true;
            this.dateTakenLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dateTakenLabel.Location = new System.Drawing.Point(6, 97);
            this.dateTakenLabel.Name = "dateTakenLabel";
            this.dateTakenLabel.Size = new System.Drawing.Size(107, 28);
            this.dateTakenLabel.TabIndex = 18;
            this.dateTakenLabel.Text = "Date Taken";
            // 
            // resetBtn
            // 
            this.resetBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.resetBtn.Location = new System.Drawing.Point(163, 436);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(154, 41);
            this.resetBtn.TabIndex = 17;
            this.resetBtn.Text = "Reset";
            this.resetBtn.UseVisualStyleBackColor = true;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // skipPhotoBtn
            // 
            this.skipPhotoBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.skipPhotoBtn.Location = new System.Drawing.Point(163, 389);
            this.skipPhotoBtn.Name = "skipPhotoBtn";
            this.skipPhotoBtn.Size = new System.Drawing.Size(154, 41);
            this.skipPhotoBtn.TabIndex = 16;
            this.skipPhotoBtn.Text = "Skip Photo";
            this.skipPhotoBtn.UseVisualStyleBackColor = true;
            this.skipPhotoBtn.Click += new System.EventHandler(this.skipPhotoBtn_Click);
            // 
            // deletePhotoBtn
            // 
            this.deletePhotoBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.deletePhotoBtn.Location = new System.Drawing.Point(6, 436);
            this.deletePhotoBtn.Name = "deletePhotoBtn";
            this.deletePhotoBtn.Size = new System.Drawing.Size(154, 41);
            this.deletePhotoBtn.TabIndex = 15;
            this.deletePhotoBtn.Text = "Delete Photo";
            this.deletePhotoBtn.UseVisualStyleBackColor = true;
            this.deletePhotoBtn.Click += new System.EventHandler(this.deletePhotoBtn_Click);
            // 
            // nextPhotoBtn
            // 
            this.nextPhotoBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nextPhotoBtn.Location = new System.Drawing.Point(6, 389);
            this.nextPhotoBtn.Name = "nextPhotoBtn";
            this.nextPhotoBtn.Size = new System.Drawing.Size(154, 41);
            this.nextPhotoBtn.TabIndex = 14;
            this.nextPhotoBtn.Text = "Next Photo";
            this.nextPhotoBtn.UseVisualStyleBackColor = true;
            this.nextPhotoBtn.Click += new System.EventHandler(this.nextPhotoBtn_Click);
            // 
            // photoPreview
            // 
            this.photoPreview.ImageLocation = "";
            this.photoPreview.Location = new System.Drawing.Point(357, 12);
            this.photoPreview.Name = "photoPreview";
            this.photoPreview.Size = new System.Drawing.Size(1295, 756);
            this.photoPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.photoPreview.TabIndex = 13;
            this.photoPreview.TabStop = false;
            // 
            // GraphicalPhotoOrganizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1664, 783);
            this.Controls.Add(this.photoPreview);
            this.Controls.Add(this.currentPhotoGroupBox);
            this.Controls.Add(this.setupGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GraphicalPhotoOrganizer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Graphical Photo Organizer";
            this.Shown += new System.EventHandler(this.GraphicalPhotoOrganizer_Shown);
            this.setupGroupBox.ResumeLayout(false);
            this.setupGroupBox.PerformLayout();
            this.currentPhotoGroupBox.ResumeLayout(false);
            this.currentPhotoGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.photoPreview)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion
    private MonthCalendar datePicker;
    private Label filenameLabel;
    private TextBox filenameTextBox;
    private Button chooseSrcDirBtn;
    private Label srcDirLabel;
    private GroupBox setupGroupBox;
    private Button chooseDestDirBtn;
    private Label destDirLabel;
    private GroupBox currentPhotoGroupBox;
    private PictureBox photoPreview;
    private Button nextPhotoBtn;
    private Button skipPhotoBtn;
    private Button deletePhotoBtn;
    private Label destPathLabel;
    private Button resetBtn;
    private Label dateTakenLabel;
    private Button beginBtn;
    private Label dateTakenSrcLabel;
}
