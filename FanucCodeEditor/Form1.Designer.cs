namespace FanucCodeEditor
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
            this.buttonFile = new System.Windows.Forms.Button();
            this.label = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.buttonSeparate = new System.Windows.Forms.Button();
            this.buttonCombine = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonFile
            // 
            this.buttonFile.Location = new System.Drawing.Point(404, 25);
            this.buttonFile.Name = "buttonFile";
            this.buttonFile.Size = new System.Drawing.Size(28, 20);
            this.buttonFile.TabIndex = 0;
            this.buttonFile.Text = "...";
            this.buttonFile.UseVisualStyleBackColor = true;
            this.buttonFile.Click += new System.EventHandler(this.buttonFile_Click);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(12, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(89, 13);
            this.label.TabIndex = 1;
            this.label.Text = ".LS File Location:";
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(12, 25);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(386, 20);
            this.textBox.TabIndex = 2;
            // 
            // buttonSeparate
            // 
            this.buttonSeparate.Location = new System.Drawing.Point(12, 51);
            this.buttonSeparate.Name = "buttonSeparate";
            this.buttonSeparate.Size = new System.Drawing.Size(137, 74);
            this.buttonSeparate.TabIndex = 3;
            this.buttonSeparate.Text = "Separate";
            this.buttonSeparate.UseVisualStyleBackColor = true;
            this.buttonSeparate.Click += new System.EventHandler(this.buttonSeparate_Click);
            // 
            // buttonCombine
            // 
            this.buttonCombine.Location = new System.Drawing.Point(295, 51);
            this.buttonCombine.Name = "buttonCombine";
            this.buttonCombine.Size = new System.Drawing.Size(137, 74);
            this.buttonCombine.TabIndex = 4;
            this.buttonCombine.Text = "Combine";
            this.buttonCombine.UseVisualStyleBackColor = true;
            this.buttonCombine.Click += new System.EventHandler(this.buttonCombine_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 134);
            this.Controls.Add(this.buttonCombine);
            this.Controls.Add(this.buttonSeparate);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.label);
            this.Controls.Add(this.buttonFile);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonFile;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button buttonSeparate;
        private System.Windows.Forms.Button buttonCombine;
    }
}

