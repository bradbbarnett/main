namespace LadderCompareV3
{
    partial class Main
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
            this.checkBoxDebug = new System.Windows.Forms.CheckBox();
            this.groupBoxOrientation = new System.Windows.Forms.GroupBox();
            this.radioButtonPortrait = new System.Windows.Forms.RadioButton();
            this.radioButtonLandscape = new System.Windows.Forms.RadioButton();
            this.LabelStatus = new System.Windows.Forms.Label();
            this.ButtonAfter = new System.Windows.Forms.Button();
            this.TextboxAfter = new System.Windows.Forms.TextBox();
            this.TextboxBefore = new System.Windows.Forms.TextBox();
            this.ButtonBefore = new System.Windows.Forms.Button();
            this.LabelAfter = new System.Windows.Forms.Label();
            this.LabelBefore = new System.Windows.Forms.Label();
            this.ButtonCompare = new System.Windows.Forms.Button();
            this.FolderBrowserDialogAfter = new System.Windows.Forms.FolderBrowserDialog();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.backgroundText = new System.ComponentModel.BackgroundWorker();
            this.FolderBrowserDialogBefore = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxOrientation.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxDebug
            // 
            this.checkBoxDebug.AutoSize = true;
            this.checkBoxDebug.Location = new System.Drawing.Point(125, 155);
            this.checkBoxDebug.Name = "checkBoxDebug";
            this.checkBoxDebug.Size = new System.Drawing.Size(56, 17);
            this.checkBoxDebug.TabIndex = 58;
            this.checkBoxDebug.Text = "debug";
            this.checkBoxDebug.UseVisualStyleBackColor = true;
            // 
            // groupBoxOrientation
            // 
            this.groupBoxOrientation.Controls.Add(this.radioButtonPortrait);
            this.groupBoxOrientation.Controls.Add(this.radioButtonLandscape);
            this.groupBoxOrientation.Location = new System.Drawing.Point(187, 112);
            this.groupBoxOrientation.Name = "groupBoxOrientation";
            this.groupBoxOrientation.Size = new System.Drawing.Size(105, 67);
            this.groupBoxOrientation.TabIndex = 57;
            this.groupBoxOrientation.TabStop = false;
            this.groupBoxOrientation.Text = "Orientation";
            // 
            // radioButtonPortrait
            // 
            this.radioButtonPortrait.AutoSize = true;
            this.radioButtonPortrait.Checked = true;
            this.radioButtonPortrait.Location = new System.Drawing.Point(6, 25);
            this.radioButtonPortrait.Name = "radioButtonPortrait";
            this.radioButtonPortrait.Size = new System.Drawing.Size(58, 17);
            this.radioButtonPortrait.TabIndex = 34;
            this.radioButtonPortrait.TabStop = true;
            this.radioButtonPortrait.Text = "Portrait";
            this.radioButtonPortrait.UseVisualStyleBackColor = true;
            // 
            // radioButtonLandscape
            // 
            this.radioButtonLandscape.AutoSize = true;
            this.radioButtonLandscape.Location = new System.Drawing.Point(6, 43);
            this.radioButtonLandscape.Name = "radioButtonLandscape";
            this.radioButtonLandscape.Size = new System.Drawing.Size(78, 17);
            this.radioButtonLandscape.TabIndex = 35;
            this.radioButtonLandscape.Text = "Landscape";
            this.radioButtonLandscape.UseVisualStyleBackColor = true;
            // 
            // LabelStatus
            // 
            this.LabelStatus.AutoSize = true;
            this.LabelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelStatus.Location = new System.Drawing.Point(9, 124);
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Size = new System.Drawing.Size(0, 20);
            this.LabelStatus.TabIndex = 56;
            // 
            // ButtonAfter
            // 
            this.ButtonAfter.Location = new System.Drawing.Point(394, 75);
            this.ButtonAfter.Name = "ButtonAfter";
            this.ButtonAfter.Size = new System.Drawing.Size(28, 20);
            this.ButtonAfter.TabIndex = 54;
            this.ButtonAfter.Text = "...";
            // 
            // TextboxAfter
            // 
            this.TextboxAfter.AllowDrop = true;
            this.TextboxAfter.Location = new System.Drawing.Point(12, 75);
            this.TextboxAfter.Name = "TextboxAfter";
            this.TextboxAfter.Size = new System.Drawing.Size(376, 20);
            this.TextboxAfter.TabIndex = 53;
            this.TextboxAfter.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextboxAfter_DragDrop);
            this.TextboxAfter.DragOver += new System.Windows.Forms.DragEventHandler(this.TextboxAfter_DragOver);
            // 
            // TextboxBefore
            // 
            this.TextboxBefore.AllowDrop = true;
            this.TextboxBefore.Location = new System.Drawing.Point(12, 26);
            this.TextboxBefore.Name = "TextboxBefore";
            this.TextboxBefore.Size = new System.Drawing.Size(376, 20);
            this.TextboxBefore.TabIndex = 50;
            this.TextboxBefore.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextboxBefore_DragDrop);
            this.TextboxBefore.DragOver += new System.Windows.Forms.DragEventHandler(this.TextboxBefore_DragOver);
            // 
            // ButtonBefore
            // 
            this.ButtonBefore.Location = new System.Drawing.Point(394, 25);
            this.ButtonBefore.Name = "ButtonBefore";
            this.ButtonBefore.Size = new System.Drawing.Size(28, 20);
            this.ButtonBefore.TabIndex = 51;
            this.ButtonBefore.Text = "...";
            // 
            // LabelAfter
            // 
            this.LabelAfter.Location = new System.Drawing.Point(9, 58);
            this.LabelAfter.Name = "LabelAfter";
            this.LabelAfter.Size = new System.Drawing.Size(85, 15);
            this.LabelAfter.TabIndex = 52;
            this.LabelAfter.Text = "Ladder After:";
            // 
            // LabelBefore
            // 
            this.LabelBefore.Location = new System.Drawing.Point(9, 8);
            this.LabelBefore.Name = "LabelBefore";
            this.LabelBefore.Size = new System.Drawing.Size(85, 15);
            this.LabelBefore.TabIndex = 49;
            this.LabelBefore.Text = "Ladder Before:";
            // 
            // ButtonCompare
            // 
            this.ButtonCompare.Location = new System.Drawing.Point(298, 112);
            this.ButtonCompare.Name = "ButtonCompare";
            this.ButtonCompare.Size = new System.Drawing.Size(124, 67);
            this.ButtonCompare.TabIndex = 55;
            this.ButtonCompare.Text = "Compare";
            this.ButtonCompare.Click += new System.EventHandler(this.ButtonCompare_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            // 
            // backgroundText
            // 
            this.backgroundText.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundText_DoWork);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 187);
            this.Controls.Add(this.checkBoxDebug);
            this.Controls.Add(this.groupBoxOrientation);
            this.Controls.Add(this.LabelStatus);
            this.Controls.Add(this.ButtonAfter);
            this.Controls.Add(this.TextboxAfter);
            this.Controls.Add(this.TextboxBefore);
            this.Controls.Add(this.ButtonBefore);
            this.Controls.Add(this.LabelAfter);
            this.Controls.Add(this.LabelBefore);
            this.Controls.Add(this.ButtonCompare);
            this.Name = "Main";
            this.Text = "Form1";
            this.groupBoxOrientation.ResumeLayout(false);
            this.groupBoxOrientation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxDebug;
        private System.Windows.Forms.GroupBox groupBoxOrientation;
        private System.Windows.Forms.RadioButton radioButtonPortrait;
        private System.Windows.Forms.RadioButton radioButtonLandscape;
        private System.Windows.Forms.Label LabelStatus;
        private System.Windows.Forms.Button ButtonAfter;
        private System.Windows.Forms.TextBox TextboxAfter;
        private System.Windows.Forms.TextBox TextboxBefore;
        private System.Windows.Forms.Button ButtonBefore;
        private System.Windows.Forms.Label LabelAfter;
        private System.Windows.Forms.Label LabelBefore;
        private System.Windows.Forms.Button ButtonCompare;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialogAfter;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.ComponentModel.BackgroundWorker backgroundText;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialogBefore;
    }
}

