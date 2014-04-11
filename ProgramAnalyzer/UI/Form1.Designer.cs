namespace UI
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
            this.openFileDialogAttributes = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowseAttributes = new System.Windows.Forms.Button();
            this.txtAttributesFileName = new System.Windows.Forms.TextBox();
            this.openFileDialogProgramText = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowseProgramText = new System.Windows.Forms.Button();
            this.txtProgramTextFileName = new System.Windows.Forms.TextBox();
            this.btnRunAnalysis = new System.Windows.Forms.Button();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // openFileDialogAttributes
            // 
            this.openFileDialogAttributes.FileName = "openFileDialog1";
            // 
            // btnBrowseAttributes
            // 
            this.btnBrowseAttributes.Location = new System.Drawing.Point(440, 60);
            this.btnBrowseAttributes.Name = "btnBrowseAttributes";
            this.btnBrowseAttributes.Size = new System.Drawing.Size(169, 23);
            this.btnBrowseAttributes.TabIndex = 0;
            this.btnBrowseAttributes.Text = "Browse Attributes";
            this.btnBrowseAttributes.UseVisualStyleBackColor = true;
            this.btnBrowseAttributes.Click += new System.EventHandler(this.btnBrowseAttributes_Click);
            // 
            // txtAttributesFileName
            // 
            this.txtAttributesFileName.Location = new System.Drawing.Point(49, 61);
            this.txtAttributesFileName.Name = "txtAttributesFileName";
            this.txtAttributesFileName.Size = new System.Drawing.Size(368, 22);
            this.txtAttributesFileName.TabIndex = 3;
            // 
            // openFileDialogProgramText
            // 
            this.openFileDialogProgramText.FileName = "openFileDialog2";
            // 
            // btnBrowseProgramText
            // 
            this.btnBrowseProgramText.Location = new System.Drawing.Point(440, 119);
            this.btnBrowseProgramText.Name = "btnBrowseProgramText";
            this.btnBrowseProgramText.Size = new System.Drawing.Size(169, 23);
            this.btnBrowseProgramText.TabIndex = 2;
            this.btnBrowseProgramText.Text = "Browse Program Text";
            this.btnBrowseProgramText.UseVisualStyleBackColor = true;
            this.btnBrowseProgramText.Click += new System.EventHandler(this.btnBrowseProgramText_Click);
            // 
            // txtProgramTextFileName
            // 
            this.txtProgramTextFileName.Location = new System.Drawing.Point(49, 120);
            this.txtProgramTextFileName.Name = "txtProgramTextFileName";
            this.txtProgramTextFileName.Size = new System.Drawing.Size(368, 22);
            this.txtProgramTextFileName.TabIndex = 4;
            // 
            // btnRunAnalysis
            // 
            this.btnRunAnalysis.Location = new System.Drawing.Point(440, 171);
            this.btnRunAnalysis.Name = "btnRunAnalysis";
            this.btnRunAnalysis.Size = new System.Drawing.Size(169, 23);
            this.btnRunAnalysis.TabIndex = 5;
            this.btnRunAnalysis.Text = "Run Analysis";
            this.btnRunAnalysis.UseVisualStyleBackColor = true;
            this.btnRunAnalysis.Click += new System.EventHandler(this.btnRunAnalysis_Click);
            // 
            // txtResults
            // 
            this.txtResults.Location = new System.Drawing.Point(63, 252);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.Size = new System.Drawing.Size(890, 382);
            this.txtResults.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 671);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.btnRunAnalysis);
            this.Controls.Add(this.txtProgramTextFileName);
            this.Controls.Add(this.btnBrowseProgramText);
            this.Controls.Add(this.txtAttributesFileName);
            this.Controls.Add(this.btnBrowseAttributes);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialogAttributes;
        private System.Windows.Forms.Button btnBrowseAttributes;
        private System.Windows.Forms.TextBox txtAttributesFileName;
        private System.Windows.Forms.OpenFileDialog openFileDialogProgramText;
        private System.Windows.Forms.Button btnBrowseProgramText;
        private System.Windows.Forms.TextBox txtProgramTextFileName;
        private System.Windows.Forms.Button btnRunAnalysis;
        private System.Windows.Forms.TextBox txtResults;

    }
}

