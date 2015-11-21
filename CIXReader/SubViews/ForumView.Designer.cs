namespace CIXReader.SubViews
{
    internal sealed partial class ForumsView
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
            this.frmCanvas = new CIXReader.Canvas.Canvas();
            this.SuspendLayout();
            // 
            // frmCanvas
            // 
            this.frmCanvas.AllowSelection = false;
            this.frmCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frmCanvas.AutoScroll = true;
            this.frmCanvas.AutoScrollMinSize = new System.Drawing.Size(421, 0);
            this.frmCanvas.CommentColour = System.Drawing.Color.Empty;
            this.frmCanvas.DisableMarkup = false;
            this.frmCanvas.ExpandInlineImages = true;
            this.frmCanvas.HasImages = true;
            this.frmCanvas.IndentationOffset = 10;
            this.frmCanvas.IsLayoutSuspended = false;
            this.frmCanvas.Location = new System.Drawing.Point(0, 0);
            this.frmCanvas.Name = "frmCanvas";
            this.frmCanvas.SelectedItem = null;
            this.frmCanvas.SelectionColour = System.Drawing.SystemColors.Highlight;
            this.frmCanvas.SeparatorColour = System.Drawing.SystemColors.GrayText;
            this.frmCanvas.Size = new System.Drawing.Size(421, 500);
            this.frmCanvas.TabIndex = 0;
            // 
            // ForumsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.frmCanvas);
            this.Name = "ForumsView";
            this.Size = new System.Drawing.Size(424, 500);
            this.Load += new System.EventHandler(this.ForumView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CIXReader.Canvas.Canvas frmCanvas;
    }
}
