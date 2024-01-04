namespace FileSelectExample
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
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.buttonFilePath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.Location = new System.Drawing.Point(12, 23);
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(206, 20);
            this.textBoxFilePath.TabIndex = 9;
            // 
            // buttonFilePath
            // 
            this.buttonFilePath.Location = new System.Drawing.Point(224, 23);
            this.buttonFilePath.Name = "buttonFilePath";
            this.buttonFilePath.Size = new System.Drawing.Size(28, 20);
            this.buttonFilePath.TabIndex = 8;
            this.buttonFilePath.Text = "...";
            this.buttonFilePath.UseVisualStyleBackColor = true;
            this.buttonFilePath.Click += new System.EventHandler(this.ButtonFilePath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "File Path:";
            // 
            // buttonExecute
            // 
            this.buttonExecute.Location = new System.Drawing.Point(95, 49);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(72, 44);
            this.buttonExecute.TabIndex = 10;
            this.buttonExecute.Text = "Verify Path";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonVerifyPath_Click);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 104);
            this.Controls.Add(this.buttonExecute);
            this.Controls.Add(this.textBoxFilePath);
            this.Controls.Add(this.buttonFilePath);
            this.Controls.Add(this.label1);
            this.Name = "Form";
            this.Text = "File Selector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxFilePath;
        private System.Windows.Forms.Button buttonFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonExecute;
    }
}

