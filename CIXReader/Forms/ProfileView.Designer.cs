namespace CIXReader.Forms
{
    internal sealed partial class ProfileView
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
            this.prvDetails = new CIXReader.Canvas.Canvas();
            this.SuspendLayout();
            // 
            // prvDetails
            // 
            this.prvDetails.AllowSelection = false;
            this.prvDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prvDetails.AutoScroll = true;
            this.prvDetails.AutoScrollMinSize = new System.Drawing.Size(471, 0);
            this.prvDetails.CommentColour = System.Drawing.Color.Empty;
            this.prvDetails.DisableMarkup = false;
            this.prvDetails.ExpandInlineImages = true;
            this.prvDetails.HasImages = true;
            this.prvDetails.IndentationOffset = 10;
            this.prvDetails.IsLayoutSuspended = false;
            this.prvDetails.Location = new System.Drawing.Point(0, 0);
            this.prvDetails.Name = "prvDetails";
            this.prvDetails.SelectedItem = null;
            this.prvDetails.SelectionColour = System.Drawing.SystemColors.Highlight;
            this.prvDetails.SeparatorColour = System.Drawing.SystemColors.GrayText;
            this.prvDetails.Size = new System.Drawing.Size(471, 528);
            this.prvDetails.TabIndex = 0;
            // 
            // ProfileView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 529);
            this.Controls.Add(this.prvDetails);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "ProfileView";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Deactivate += new System.EventHandler(this.ProfileView_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProfileView_FormClosing);
            this.Load += new System.EventHandler(this.ProfileView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CIXReader.Canvas.Canvas prvDetails;
    }
}