namespace MemoryLadGX
{
    partial class Form
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
            this.Textbox = new System.Windows.Forms.TextBox();
            this.ButtonFile = new System.Windows.Forms.Button();
            this.Label = new System.Windows.Forms.Label();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // Textbox
            // 
            this.Textbox.AllowDrop = true;
            this.Textbox.Location = new System.Drawing.Point(12, 31);
            this.Textbox.Name = "Textbox";
            this.Textbox.Size = new System.Drawing.Size(376, 20);
            this.Textbox.TabIndex = 39;
            this.Textbox.DragDrop += new System.Windows.Forms.DragEventHandler(this.Textbox_DragDrop);
            this.Textbox.DragOver += new System.Windows.Forms.DragEventHandler(this.Textbox_DragOver);
            // 
            // ButtonFile
            // 
            this.ButtonFile.Location = new System.Drawing.Point(394, 30);
            this.ButtonFile.Name = "ButtonFile";
            this.ButtonFile.Size = new System.Drawing.Size(28, 20);
            this.ButtonFile.TabIndex = 40;
            this.ButtonFile.Text = "...";
            this.ButtonFile.Click += new System.EventHandler(this.ButtonFile_Click);
            // 
            // Label
            // 
            this.Label.Location = new System.Drawing.Point(9, 13);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(85, 15);
            this.Label.TabIndex = 38;
            this.Label.Text = "Memory File:";
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(298, 67);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(124, 67);
            this.ButtonStart.TabIndex = 44;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 142);
            this.Controls.Add(this.Textbox);
            this.Controls.Add(this.ButtonFile);
            this.Controls.Add(this.Label);
            this.Controls.Add(this.ButtonStart);
            this.Name = "Form";
            this.Text = "GX Developer Memory File Bot";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox Textbox;
        private System.Windows.Forms.Button ButtonFile;
        private System.Windows.Forms.Label Label;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog;
    }
}

