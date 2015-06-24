namespace Label_Viewer
{
    partial class FileList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileList));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelList = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.RevertButton = new System.Windows.Forms.Button();
            this.printAllButton = new System.Windows.Forms.Button();
            this.previewButton = new System.Windows.Forms.Button();
            this.selectAllButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.labelList);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 194);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AVAILABLE LABEL FILES";
            // 
            // labelList
            // 
            this.labelList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelList.FormattingEnabled = true;
            this.labelList.Location = new System.Drawing.Point(19, 19);
            this.labelList.Name = "labelList";
            this.labelList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.labelList.Size = new System.Drawing.Size(342, 147);
            this.labelList.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.closeButton);
            this.groupBox2.Controls.Add(this.RevertButton);
            this.groupBox2.Controls.Add(this.printAllButton);
            this.groupBox2.Controls.Add(this.previewButton);
            this.groupBox2.Controls.Add(this.selectAllButton);
            this.groupBox2.Controls.Add(this.settingsButton);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(399, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(235, 194);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ACTIONS";
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.Location = new System.Drawing.Point(124, 147);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(85, 23);
            this.closeButton.TabIndex = 6;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // RevertButton
            // 
            this.RevertButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RevertButton.Location = new System.Drawing.Point(24, 147);
            this.RevertButton.Name = "RevertButton";
            this.RevertButton.Size = new System.Drawing.Size(85, 23);
            this.RevertButton.TabIndex = 5;
            this.RevertButton.Text = "Revert";
            this.RevertButton.UseVisualStyleBackColor = true;
            this.RevertButton.Click += new System.EventHandler(this.RevertButton_Click);
            // 
            // printAllButton
            // 
            this.printAllButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printAllButton.Location = new System.Drawing.Point(124, 91);
            this.printAllButton.Name = "printAllButton";
            this.printAllButton.Size = new System.Drawing.Size(85, 23);
            this.printAllButton.TabIndex = 3;
            this.printAllButton.Text = "Print as PDF";
            this.printAllButton.UseVisualStyleBackColor = true;
            this.printAllButton.Click += new System.EventHandler(this.printAllButton_Click);
            // 
            // previewButton
            // 
            this.previewButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previewButton.Location = new System.Drawing.Point(24, 91);
            this.previewButton.Name = "previewButton";
            this.previewButton.Size = new System.Drawing.Size(85, 23);
            this.previewButton.TabIndex = 2;
            this.previewButton.Text = "Preview";
            this.previewButton.UseVisualStyleBackColor = true;
            this.previewButton.Click += new System.EventHandler(this.startPollingButton_Click);
            // 
            // selectAllButton
            // 
            this.selectAllButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectAllButton.Location = new System.Drawing.Point(124, 33);
            this.selectAllButton.Name = "selectAllButton";
            this.selectAllButton.Size = new System.Drawing.Size(85, 23);
            this.selectAllButton.TabIndex = 1;
            this.selectAllButton.Text = "Select All";
            this.selectAllButton.UseVisualStyleBackColor = true;
            this.selectAllButton.Click += new System.EventHandler(this.selectAllButton_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsButton.Location = new System.Drawing.Point(24, 33);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(85, 23);
            this.settingsButton.TabIndex = 0;
            this.settingsButton.Text = "Settings";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // FileList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.closeButton;
            this.ClientSize = new System.Drawing.Size(646, 224);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FileList";
            this.Text = "Available Label Files";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox labelList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button RevertButton;
        private System.Windows.Forms.Button printAllButton;
        private System.Windows.Forms.Button previewButton;
        private System.Windows.Forms.Button selectAllButton;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.PrintDialog printDialog1;
    }
}