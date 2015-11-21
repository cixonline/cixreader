using CIXReader.Properties;

namespace CIXReader.SubViews
{
    internal sealed partial class WelcomeView
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
            this.wlcTopPosts = new CIXReader.Canvas.Canvas();
            this.SuspendLayout();
            // 
            // wlcTopPosts
            // 
            this.wlcTopPosts.AllowSelection = false;
            this.wlcTopPosts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wlcTopPosts.AutoScroll = true;
            this.wlcTopPosts.AutoScrollMinSize = new System.Drawing.Size(75, 0);
            this.wlcTopPosts.CommentColour = System.Drawing.Color.Empty;
            this.wlcTopPosts.DisableMarkup = false;
            this.wlcTopPosts.ExpandInlineImages = false;
            this.wlcTopPosts.HasImages = true;
            this.wlcTopPosts.IndentationOffset = 0;
            this.wlcTopPosts.IsLayoutSuspended = false;
            this.wlcTopPosts.Location = new System.Drawing.Point(5, 5);
            this.wlcTopPosts.Name = "wlcTopPosts";
            this.wlcTopPosts.SelectedItem = null;
            this.wlcTopPosts.SelectionColour = System.Drawing.SystemColors.Highlight;
            this.wlcTopPosts.SeparatorColour = System.Drawing.SystemColors.GrayText;
            this.wlcTopPosts.Size = new System.Drawing.Size(490, 530);
            this.wlcTopPosts.TabIndex = 3;
            // 
            // WelcomeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.wlcTopPosts);
            this.Name = "WelcomeView";
            this.Size = new System.Drawing.Size(505, 542);
            this.Load += new System.EventHandler(this.WelcomeView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Canvas.Canvas wlcTopPosts;
    }
}