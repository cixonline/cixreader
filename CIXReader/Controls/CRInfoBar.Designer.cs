namespace CIXReader.Controls
{
    sealed partial class CRInfoBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.icon = new System.Windows.Forms.PictureBox();
            this.text = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            this.SuspendLayout();
            // 
            // icon
            // 
            this.icon.Location = new System.Drawing.Point(8, 6);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(21, 17);
            this.icon.TabIndex = 3;
            this.icon.TabStop = false;
            this.icon.Visible = false;
            // 
            // text
            // 
            this.text.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.text.AutoSize = true;
            this.text.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text.ForeColor = System.Drawing.SystemColors.ControlText;
            this.text.Location = new System.Drawing.Point(8, 5);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(0, 17);
            this.text.TabIndex = 2;
            // 
            // CRInfoBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Cornsilk;
            this.Controls.Add(this.icon);
            this.Controls.Add(this.text);
            this.Name = "CRInfoBar";
            this.Size = new System.Drawing.Size(422, 28);
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox icon;
        private System.Windows.Forms.Label text;

    }
}
