namespace CIXReader.Forms {
    partial class SyncProgress {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncProgress));
            this.crPanel1 = new CIXReader.Controls.CRPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.abtLogo = new System.Windows.Forms.PictureBox();
            this.abtClose = new System.Windows.Forms.Button();
            this.crPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.abtLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // crPanel1
            // 
            this.crPanel1.BackColor = System.Drawing.SystemColors.Window;
            this.crPanel1.BottomBorderWidth = 1;
            this.crPanel1.Controls.Add(this.label2);
            this.crPanel1.Controls.Add(this.label1);
            this.crPanel1.Controls.Add(this.abtLogo);
            this.crPanel1.Gradient = false;
            this.crPanel1.LeftBorderWidth = 0;
            this.crPanel1.Location = new System.Drawing.Point(0, 0);
            this.crPanel1.Name = "crPanel1";
            this.crPanel1.RightBorderWidth = 0;
            this.crPanel1.Size = new System.Drawing.Size(374, 127);
            this.crPanel1.TabIndex = 0;
            this.crPanel1.TopBorderWidth = 0;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(137, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(222, 69);
            this.label2.TabIndex = 10;
            this.label2.Text = "The database is being set up with your forums and messages. Please wait for this " +
    "to complete.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(136, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "CIXReader is initialising...";
            // 
            // abtLogo
            // 
            this.abtLogo.BackColor = System.Drawing.SystemColors.Window;
            this.abtLogo.Image = global::CIXReader.Properties.Resources.CIXReaderLogo;
            this.abtLogo.InitialImage = ((System.Drawing.Image)(resources.GetObject("abtLogo.InitialImage")));
            this.abtLogo.Location = new System.Drawing.Point(12, 12);
            this.abtLogo.Name = "abtLogo";
            this.abtLogo.Size = new System.Drawing.Size(100, 100);
            this.abtLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.abtLogo.TabIndex = 8;
            this.abtLogo.TabStop = false;
            // 
            // abtClose
            // 
            this.abtClose.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.abtClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.abtClose.Font = new System.Drawing.Font("Arial", 8F);
            this.abtClose.Location = new System.Drawing.Point(284, 134);
            this.abtClose.Name = "abtClose";
            this.abtClose.Size = new System.Drawing.Size(75, 23);
            this.abtClose.TabIndex = 14;
            this.abtClose.Text = "Exit";
            this.abtClose.UseVisualStyleBackColor = false;
            // 
            // SyncProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 166);
            this.ControlBox = false;
            this.Controls.Add(this.abtClose);
            this.Controls.Add(this.crPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SyncProgress";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Initialising...";
            this.Shown += new System.EventHandler(this.SyncProgress_Shown);
            this.crPanel1.ResumeLayout(false);
            this.crPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.abtLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CRPanel crPanel1;
        private System.Windows.Forms.PictureBox abtLogo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button abtClose;
    }
}