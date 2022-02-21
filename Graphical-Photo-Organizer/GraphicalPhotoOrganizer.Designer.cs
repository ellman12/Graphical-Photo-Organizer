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
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.datePickerLabel = new System.Windows.Forms.Label();
            this.filenameLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.chooseSrcDirBtn = new System.Windows.Forms.Button();
            this.srcDirLabel = new System.Windows.Forms.Label();
            this.setupGroupBox = new System.Windows.Forms.GroupBox();
            this.chooseDestDirBtn = new System.Windows.Forms.Button();
            this.destDirLabel = new System.Windows.Forms.Label();
            this.destPathLabel = new System.Windows.Forms.Label();
            this.currentItemGroupBox = new System.Windows.Forms.GroupBox();
            this.resetBtn = new System.Windows.Forms.Button();
            this.skipPhotoBtn = new System.Windows.Forms.Button();
            this.deletePhotoBtn = new System.Windows.Forms.Button();
            this.nextPhotoBtn = new System.Windows.Forms.Button();
            this.applyBtn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.setupGroupBox.SuspendLayout();
            this.currentItemGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.monthCalendar1.Location = new System.Drawing.Point(6, 123);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 1;
            // 
            // datePickerLabel
            // 
            this.datePickerLabel.AutoSize = true;
            this.datePickerLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.datePickerLabel.Location = new System.Drawing.Point(6, 94);
            this.datePickerLabel.Name = "datePickerLabel";
            this.datePickerLabel.Size = new System.Drawing.Size(107, 28);
            this.datePickerLabel.TabIndex = 2;
            this.datePickerLabel.Text = "Date Taken";
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
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.Location = new System.Drawing.Point(6, 57);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(312, 34);
            this.textBox1.TabIndex = 4;
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
            this.srcDirLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.srcDirLabel.Location = new System.Drawing.Point(6, 80);
            this.srcDirLabel.Name = "srcDirLabel";
            this.srcDirLabel.Size = new System.Drawing.Size(36, 28);
            this.srcDirLabel.TabIndex = 9;
            this.srcDirLabel.Text = "dir";
            // 
            // setupGroupBox
            // 
            this.setupGroupBox.Controls.Add(this.chooseDestDirBtn);
            this.setupGroupBox.Controls.Add(this.destDirLabel);
            this.setupGroupBox.Controls.Add(this.chooseSrcDirBtn);
            this.setupGroupBox.Controls.Add(this.srcDirLabel);
            this.setupGroupBox.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.setupGroupBox.Location = new System.Drawing.Point(12, 12);
            this.setupGroupBox.Name = "setupGroupBox";
            this.setupGroupBox.Size = new System.Drawing.Size(326, 186);
            this.setupGroupBox.TabIndex = 10;
            this.setupGroupBox.TabStop = false;
            this.setupGroupBox.Text = "Setup";
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
            this.destDirLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.destDirLabel.Location = new System.Drawing.Point(6, 155);
            this.destDirLabel.Name = "destDirLabel";
            this.destDirLabel.Size = new System.Drawing.Size(36, 28);
            this.destDirLabel.TabIndex = 12;
            this.destDirLabel.Text = "dir";
            // 
            // destPathLabel
            // 
            this.destPathLabel.AutoSize = true;
            this.destPathLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.destPathLabel.Location = new System.Drawing.Point(6, 519);
            this.destPathLabel.Name = "destPathLabel";
            this.destPathLabel.Size = new System.Drawing.Size(80, 25);
            this.destPathLabel.TabIndex = 6;
            this.destPathLabel.Text = "destPath";
            // 
            // currentItemGroupBox
            // 
            this.currentItemGroupBox.Controls.Add(this.resetBtn);
            this.currentItemGroupBox.Controls.Add(this.skipPhotoBtn);
            this.currentItemGroupBox.Controls.Add(this.deletePhotoBtn);
            this.currentItemGroupBox.Controls.Add(this.destPathLabel);
            this.currentItemGroupBox.Controls.Add(this.nextPhotoBtn);
            this.currentItemGroupBox.Controls.Add(this.applyBtn);
            this.currentItemGroupBox.Controls.Add(this.filenameLabel);
            this.currentItemGroupBox.Controls.Add(this.textBox1);
            this.currentItemGroupBox.Controls.Add(this.monthCalendar1);
            this.currentItemGroupBox.Controls.Add(this.datePickerLabel);
            this.currentItemGroupBox.Enabled = false;
            this.currentItemGroupBox.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.currentItemGroupBox.Location = new System.Drawing.Point(12, 204);
            this.currentItemGroupBox.Name = "currentItemGroupBox";
            this.currentItemGroupBox.Size = new System.Drawing.Size(326, 553);
            this.currentItemGroupBox.TabIndex = 11;
            this.currentItemGroupBox.TabStop = false;
            this.currentItemGroupBox.Text = "Current Photo";
            // 
            // resetBtn
            // 
            this.resetBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.resetBtn.Location = new System.Drawing.Point(6, 476);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(154, 41);
            this.resetBtn.TabIndex = 17;
            this.resetBtn.Text = "Reset";
            this.resetBtn.UseVisualStyleBackColor = true;
            // 
            // skipPhotoBtn
            // 
            this.skipPhotoBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.skipPhotoBtn.Location = new System.Drawing.Point(164, 429);
            this.skipPhotoBtn.Name = "skipPhotoBtn";
            this.skipPhotoBtn.Size = new System.Drawing.Size(154, 41);
            this.skipPhotoBtn.TabIndex = 16;
            this.skipPhotoBtn.Text = "Skip Photo";
            this.skipPhotoBtn.UseVisualStyleBackColor = true;
            // 
            // deletePhotoBtn
            // 
            this.deletePhotoBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.deletePhotoBtn.Location = new System.Drawing.Point(6, 429);
            this.deletePhotoBtn.Name = "deletePhotoBtn";
            this.deletePhotoBtn.Size = new System.Drawing.Size(154, 41);
            this.deletePhotoBtn.TabIndex = 15;
            this.deletePhotoBtn.Text = "Delete Photo";
            this.deletePhotoBtn.UseVisualStyleBackColor = true;
            // 
            // nextPhotoBtn
            // 
            this.nextPhotoBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.nextPhotoBtn.Location = new System.Drawing.Point(164, 382);
            this.nextPhotoBtn.Name = "nextPhotoBtn";
            this.nextPhotoBtn.Size = new System.Drawing.Size(154, 41);
            this.nextPhotoBtn.TabIndex = 14;
            this.nextPhotoBtn.Text = "Next Photo";
            this.nextPhotoBtn.UseVisualStyleBackColor = true;
            // 
            // applyBtn
            // 
            this.applyBtn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.applyBtn.Location = new System.Drawing.Point(6, 382);
            this.applyBtn.Name = "applyBtn";
            this.applyBtn.Size = new System.Drawing.Size(154, 41);
            this.applyBtn.TabIndex = 13;
            this.applyBtn.Text = "Apply";
            this.applyBtn.UseVisualStyleBackColor = true;
            this.applyBtn.Click += new System.EventHandler(this.applyBtn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ImageLocation = "";
            this.pictureBox1.Location = new System.Drawing.Point(357, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1295, 730);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // GraphicalPhotoOrganizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1650, 767);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.currentItemGroupBox);
            this.Controls.Add(this.setupGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GraphicalPhotoOrganizer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Graphical Photo Organizer";
            this.setupGroupBox.ResumeLayout(false);
            this.setupGroupBox.PerformLayout();
            this.currentItemGroupBox.ResumeLayout(false);
            this.currentItemGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion
    private MonthCalendar monthCalendar1;
    private Label datePickerLabel;
    private Label filenameLabel;
    private TextBox textBox1;
    private Button chooseSrcDirBtn;
    private Label srcDirLabel;
    private GroupBox setupGroupBox;
    private Button chooseDestDirBtn;
    private Label destDirLabel;
    private GroupBox currentItemGroupBox;
    private PictureBox pictureBox1;
    private Button nextPhotoBtn;
    private Button applyBtn;
    private Button skipPhotoBtn;
    private Button deletePhotoBtn;
    private Label destPathLabel;
    private Button resetBtn;
}
