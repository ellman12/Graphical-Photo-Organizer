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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.datePickerLabel = new System.Windows.Forms.Label();
            this.filenameLabel = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.destPathTextLabel = new System.Windows.Forms.Label();
            this.destPathLabel = new System.Windows.Forms.Label();
            this.chooseSrcDirBtn = new System.Windows.Forms.Button();
            this.srcDirTextLabel = new System.Windows.Forms.Label();
            this.srcDirLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.ImageLocation = "C:/Users/Elliott/Pictures/Steam Screenshots/413150_20220212202151_1.png";
            this.pictureBox.Location = new System.Drawing.Point(497, 22);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(518, 292);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(12, 131);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 1;
            // 
            // datePickerLabel
            // 
            this.datePickerLabel.AutoSize = true;
            this.datePickerLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.datePickerLabel.Location = new System.Drawing.Point(12, 94);
            this.datePickerLabel.Name = "datePickerLabel";
            this.datePickerLabel.Size = new System.Drawing.Size(107, 28);
            this.datePickerLabel.TabIndex = 2;
            this.datePickerLabel.Text = "Date Taken";
            // 
            // filenameLabel
            // 
            this.filenameLabel.AutoSize = true;
            this.filenameLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.filenameLabel.Location = new System.Drawing.Point(12, 9);
            this.filenameLabel.Name = "filenameLabel";
            this.filenameLabel.Size = new System.Drawing.Size(90, 28);
            this.filenameLabel.TabIndex = 3;
            this.filenameLabel.Text = "Filename";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(312, 31);
            this.textBox1.TabIndex = 4;
            // 
            // destPathTextLabel
            // 
            this.destPathTextLabel.AutoSize = true;
            this.destPathTextLabel.Location = new System.Drawing.Point(567, 496);
            this.destPathTextLabel.Name = "destPathTextLabel";
            this.destPathTextLabel.Size = new System.Drawing.Size(150, 25);
            this.destPathTextLabel.TabIndex = 5;
            this.destPathTextLabel.Text = "Destination Path: ";
            // 
            // destPathLabel
            // 
            this.destPathLabel.AutoSize = true;
            this.destPathLabel.Location = new System.Drawing.Point(712, 496);
            this.destPathLabel.Name = "destPathLabel";
            this.destPathLabel.Size = new System.Drawing.Size(80, 25);
            this.destPathLabel.TabIndex = 6;
            this.destPathLabel.Text = "destPath";
            // 
            // chooseSrcDirBtn
            // 
            this.chooseSrcDirBtn.Location = new System.Drawing.Point(12, 522);
            this.chooseSrcDirBtn.Name = "chooseSrcDirBtn";
            this.chooseSrcDirBtn.Size = new System.Drawing.Size(199, 34);
            this.chooseSrcDirBtn.TabIndex = 7;
            this.chooseSrcDirBtn.Text = "Choose Source Folder";
            this.chooseSrcDirBtn.UseVisualStyleBackColor = true;
            this.chooseSrcDirBtn.Click += new System.EventHandler(this.chooseSrcDirBtn_Click);
            // 
            // srcDirTextLabel
            // 
            this.srcDirTextLabel.AutoSize = true;
            this.srcDirTextLabel.Location = new System.Drawing.Point(14, 586);
            this.srcDirTextLabel.Name = "srcDirTextLabel";
            this.srcDirTextLabel.Size = new System.Drawing.Size(98, 25);
            this.srcDirTextLabel.TabIndex = 8;
            this.srcDirTextLabel.Text = "Source Dir:";
            // 
            // srcDirLabel
            // 
            this.srcDirLabel.AutoSize = true;
            this.srcDirLabel.Location = new System.Drawing.Point(113, 586);
            this.srcDirLabel.Name = "srcDirLabel";
            this.srcDirLabel.Size = new System.Drawing.Size(33, 25);
            this.srcDirLabel.TabIndex = 9;
            this.srcDirLabel.Text = "dir";
            // 
            // GraphicalPhotoOrganizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1206, 756);
            this.Controls.Add(this.srcDirLabel);
            this.Controls.Add(this.srcDirTextLabel);
            this.Controls.Add(this.chooseSrcDirBtn);
            this.Controls.Add(this.destPathLabel);
            this.Controls.Add(this.destPathTextLabel);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.filenameLabel);
            this.Controls.Add(this.datePickerLabel);
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.pictureBox);
            this.Name = "GraphicalPhotoOrganizer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Graphical Photo Organizer";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private PictureBox pictureBox;
    private MonthCalendar monthCalendar1;
    private Label datePickerLabel;
    private Label filenameLabel;
    private TextBox textBox1;
    private Label destPathTextLabel;
    private Label destPathLabel;
    private Button chooseSrcDirBtn;
    private Label srcDirTextLabel;
    private Label srcDirLabel;
}
