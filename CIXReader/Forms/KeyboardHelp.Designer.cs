using CIXReader.Controls;

namespace CIXReader.Forms
{
    internal sealed partial class KeyboardHelp
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
            this.kybdClose = new System.Windows.Forms.Button();
            this.kybdPanel = new CIXReader.Controls.CRPanel();
            this.kybdTogglePlainTextLabel = new System.Windows.Forms.Label();
            this.kybdTogglePlainText = new CIXReader.Controls.CRRoundButton();
            this.kybdSearchLabel = new System.Windows.Forms.Label();
            this.kybdSearch = new CIXReader.Controls.CRRoundButton();
            this.kybdMarkTopicReadLabel = new System.Windows.Forms.Label();
            this.kybdMarkTopicRead = new CIXReader.Controls.CRRoundButton();
            this.kybdSystemMenuLabel = new System.Windows.Forms.Label();
            this.kybdSystemMenu = new CIXReader.Controls.CRRoundButton();
            this.kybdHelpLabel = new System.Windows.Forms.Label();
            this.kybdHelp = new CIXReader.Controls.CRRoundButton();
            this.kybdSwitchingViewsLabel = new System.Windows.Forms.Label();
            this.kybdMarkReadLockLabel = new System.Windows.Forms.Label();
            this.kybdMarkReadLock = new CIXReader.Controls.CRRoundButton();
            this.kybdWithdrawLabel = new System.Windows.Forms.Label();
            this.kybdWithdraw = new CIXReader.Controls.CRRoundButton();
            this.kybdGotoLabel = new System.Windows.Forms.Label();
            this.kybdGoto = new CIXReader.Controls.CRRoundButton();
            this.kybdScrollNextUnreadLabel = new System.Windows.Forms.Label();
            this.kybdScrollNextUnread = new CIXReader.Controls.CRRoundButton();
            this.kybdAuthoringMessagesLabel = new System.Windows.Forms.Label();
            this.kybdMarkingMessagesLabel = new System.Windows.Forms.Label();
            this.kybdDelLabel = new System.Windows.Forms.Label();
            this.kybdDel = new CIXReader.Controls.CRRoundButton();
            this.kybdNextThreadLabel = new System.Windows.Forms.Label();
            this.kybdNextThread = new CIXReader.Controls.CRRoundButton();
            this.kybdPreviousThreadLabel = new System.Windows.Forms.Label();
            this.kybdPreviousThread = new CIXReader.Controls.CRRoundButton();
            this.kybdMarkStarLabel = new System.Windows.Forms.Label();
            this.kybdMarkStar = new CIXReader.Controls.CRRoundButton();
            this.kybdMarkThreadReadLabel = new System.Windows.Forms.Label();
            this.kybdMarkThreadRead = new CIXReader.Controls.CRRoundButton();
            this.kybdExpandThreadLabel = new System.Windows.Forms.Label();
            this.kybdExpandThread = new CIXReader.Controls.CRRoundButton();
            this.kybdMarkPriorityLabel = new System.Windows.Forms.Label();
            this.kybdMarkPriority = new CIXReader.Controls.CRRoundButton();
            this.kybdOriginalLabel = new System.Windows.Forms.Label();
            this.kybdOriginal = new CIXReader.Controls.CRRoundButton();
            this.kybdCommentLabel = new System.Windows.Forms.Label();
            this.kybdComment = new CIXReader.Controls.CRRoundButton();
            this.kybdSayLabel = new System.Windows.Forms.Label();
            this.kybdSay = new CIXReader.Controls.CRRoundButton();
            this.kybdNextPriorityLabel = new System.Windows.Forms.Label();
            this.kybdNextPriority = new CIXReader.Controls.CRRoundButton();
            this.kybdMarkIgnoreLabel = new System.Windows.Forms.Label();
            this.kybdMarkIgnore = new CIXReader.Controls.CRRoundButton();
            this.kybdMarkReadLabel = new System.Windows.Forms.Label();
            this.kybdMarkRead = new CIXReader.Controls.CRRoundButton();
            this.kybdPreviousLabel = new System.Windows.Forms.Label();
            this.kybdPrevious = new CIXReader.Controls.CRRoundButton();
            this.kybdNextUnreadLabel = new System.Windows.Forms.Label();
            this.kybdNextUnread = new CIXReader.Controls.CRRoundButton();
            this.kybdNavigatingMessagesLabel = new System.Windows.Forms.Label();
            this.kybdQuoteLabel = new System.Windows.Forms.Label();
            this.kybdQuote = new CIXReader.Controls.CRRoundButton();
            this.kybdPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // kybdClose
            // 
            this.kybdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kybdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.kybdClose.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdClose.Location = new System.Drawing.Point(614, 378);
            this.kybdClose.Name = "kybdClose";
            this.kybdClose.Size = new System.Drawing.Size(75, 23);
            this.kybdClose.TabIndex = 11;
            this.kybdClose.Text = "Close";
            this.kybdClose.UseVisualStyleBackColor = true;
            this.kybdClose.Click += new System.EventHandler(this.kybdClose_Click);
            // 
            // kybdPanel
            // 
            this.kybdPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kybdPanel.BackColor = System.Drawing.SystemColors.Window;
            this.kybdPanel.BottomBorderWidth = 1;
            this.kybdPanel.Controls.Add(this.kybdQuoteLabel);
            this.kybdPanel.Controls.Add(this.kybdQuote);
            this.kybdPanel.Controls.Add(this.kybdTogglePlainTextLabel);
            this.kybdPanel.Controls.Add(this.kybdTogglePlainText);
            this.kybdPanel.Controls.Add(this.kybdSearchLabel);
            this.kybdPanel.Controls.Add(this.kybdSearch);
            this.kybdPanel.Controls.Add(this.kybdMarkTopicReadLabel);
            this.kybdPanel.Controls.Add(this.kybdMarkTopicRead);
            this.kybdPanel.Controls.Add(this.kybdSystemMenuLabel);
            this.kybdPanel.Controls.Add(this.kybdSystemMenu);
            this.kybdPanel.Controls.Add(this.kybdHelpLabel);
            this.kybdPanel.Controls.Add(this.kybdHelp);
            this.kybdPanel.Controls.Add(this.kybdSwitchingViewsLabel);
            this.kybdPanel.Controls.Add(this.kybdMarkReadLockLabel);
            this.kybdPanel.Controls.Add(this.kybdMarkReadLock);
            this.kybdPanel.Controls.Add(this.kybdWithdrawLabel);
            this.kybdPanel.Controls.Add(this.kybdWithdraw);
            this.kybdPanel.Controls.Add(this.kybdGotoLabel);
            this.kybdPanel.Controls.Add(this.kybdGoto);
            this.kybdPanel.Controls.Add(this.kybdScrollNextUnreadLabel);
            this.kybdPanel.Controls.Add(this.kybdScrollNextUnread);
            this.kybdPanel.Controls.Add(this.kybdAuthoringMessagesLabel);
            this.kybdPanel.Controls.Add(this.kybdMarkingMessagesLabel);
            this.kybdPanel.Controls.Add(this.kybdDelLabel);
            this.kybdPanel.Controls.Add(this.kybdDel);
            this.kybdPanel.Controls.Add(this.kybdNextThreadLabel);
            this.kybdPanel.Controls.Add(this.kybdNextThread);
            this.kybdPanel.Controls.Add(this.kybdPreviousThreadLabel);
            this.kybdPanel.Controls.Add(this.kybdPreviousThread);
            this.kybdPanel.Controls.Add(this.kybdMarkStarLabel);
            this.kybdPanel.Controls.Add(this.kybdMarkStar);
            this.kybdPanel.Controls.Add(this.kybdMarkThreadReadLabel);
            this.kybdPanel.Controls.Add(this.kybdMarkThreadRead);
            this.kybdPanel.Controls.Add(this.kybdExpandThreadLabel);
            this.kybdPanel.Controls.Add(this.kybdExpandThread);
            this.kybdPanel.Controls.Add(this.kybdMarkPriorityLabel);
            this.kybdPanel.Controls.Add(this.kybdMarkPriority);
            this.kybdPanel.Controls.Add(this.kybdOriginalLabel);
            this.kybdPanel.Controls.Add(this.kybdOriginal);
            this.kybdPanel.Controls.Add(this.kybdCommentLabel);
            this.kybdPanel.Controls.Add(this.kybdComment);
            this.kybdPanel.Controls.Add(this.kybdSayLabel);
            this.kybdPanel.Controls.Add(this.kybdSay);
            this.kybdPanel.Controls.Add(this.kybdNextPriorityLabel);
            this.kybdPanel.Controls.Add(this.kybdNextPriority);
            this.kybdPanel.Controls.Add(this.kybdMarkIgnoreLabel);
            this.kybdPanel.Controls.Add(this.kybdMarkIgnore);
            this.kybdPanel.Controls.Add(this.kybdMarkReadLabel);
            this.kybdPanel.Controls.Add(this.kybdMarkRead);
            this.kybdPanel.Controls.Add(this.kybdPreviousLabel);
            this.kybdPanel.Controls.Add(this.kybdPrevious);
            this.kybdPanel.Controls.Add(this.kybdNextUnreadLabel);
            this.kybdPanel.Controls.Add(this.kybdNextUnread);
            this.kybdPanel.Controls.Add(this.kybdNavigatingMessagesLabel);
            this.kybdPanel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.kybdPanel.Gradient = false;
            this.kybdPanel.LeftBorderWidth = 0;
            this.kybdPanel.Location = new System.Drawing.Point(0, 0);
            this.kybdPanel.Name = "kybdPanel";
            this.kybdPanel.RightBorderWidth = 0;
            this.kybdPanel.Size = new System.Drawing.Size(701, 370);
            this.kybdPanel.TabIndex = 0;
            this.kybdPanel.TopBorderWidth = 0;
            // 
            // kybdTogglePlainTextLabel
            // 
            this.kybdTogglePlainTextLabel.AutoSize = true;
            this.kybdTogglePlainTextLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdTogglePlainTextLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdTogglePlainTextLabel.Location = new System.Drawing.Point(537, 206);
            this.kybdTogglePlainTextLabel.Name = "kybdTogglePlainTextLabel";
            this.kybdTogglePlainTextLabel.Size = new System.Drawing.Size(95, 15);
            this.kybdTogglePlainTextLabel.TabIndex = 55;
            this.kybdTogglePlainTextLabel.Text = "Toggle plain text";
            // 
            // kybdTogglePlainText
            // 
            this.kybdTogglePlainText.Active = false;
            this.kybdTogglePlainText.BackColor = System.Drawing.SystemColors.Window;
            this.kybdTogglePlainText.CanBeSelected = false;
            this.kybdTogglePlainText.CanHaveFocus = true;
            this.kybdTogglePlainText.Enabled = false;
            this.kybdTogglePlainText.ExtraData = null;
            this.kybdTogglePlainText.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdTogglePlainText.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdTogglePlainText.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdTogglePlainText.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdTogglePlainText.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdTogglePlainText.ImageScaling = false;
            this.kybdTogglePlainText.Location = new System.Drawing.Point(499, 202);
            this.kybdTogglePlainText.Name = "kybdTogglePlainText";
            this.kybdTogglePlainText.Size = new System.Drawing.Size(35, 23);
            this.kybdTogglePlainText.TabIndex = 54;
            this.kybdTogglePlainText.Text = "V";
            this.kybdTogglePlainText.UseVisualStyleBackColor = false;
            // 
            // kybdSearchLabel
            // 
            this.kybdSearchLabel.AutoSize = true;
            this.kybdSearchLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdSearchLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdSearchLabel.Location = new System.Drawing.Point(300, 334);
            this.kybdSearchLabel.Name = "kybdSearchLabel";
            this.kybdSearchLabel.Size = new System.Drawing.Size(122, 15);
            this.kybdSearchLabel.TabIndex = 53;
            this.kybdSearchLabel.Text = "Search in messages";
            // 
            // kybdSearch
            // 
            this.kybdSearch.Active = false;
            this.kybdSearch.BackColor = System.Drawing.SystemColors.Window;
            this.kybdSearch.CanBeSelected = false;
            this.kybdSearch.CanHaveFocus = true;
            this.kybdSearch.Enabled = false;
            this.kybdSearch.ExtraData = null;
            this.kybdSearch.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdSearch.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdSearch.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdSearch.ImageScaling = false;
            this.kybdSearch.Location = new System.Drawing.Point(262, 330);
            this.kybdSearch.Name = "kybdSearch";
            this.kybdSearch.Size = new System.Drawing.Size(35, 23);
            this.kybdSearch.TabIndex = 52;
            this.kybdSearch.Text = "Ctrl+F";
            this.kybdSearch.UseVisualStyleBackColor = false;
            // 
            // kybdMarkTopicReadLabel
            // 
            this.kybdMarkTopicReadLabel.AutoSize = true;
            this.kybdMarkTopicReadLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdMarkTopicReadLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkTopicReadLabel.Location = new System.Drawing.Point(537, 302);
            this.kybdMarkTopicReadLabel.Name = "kybdMarkTopicReadLabel";
            this.kybdMarkTopicReadLabel.Size = new System.Drawing.Size(110, 15);
            this.kybdMarkTopicReadLabel.TabIndex = 51;
            this.kybdMarkTopicReadLabel.Text = "Mark the topic read";
            // 
            // kybdMarkTopicRead
            // 
            this.kybdMarkTopicRead.Active = false;
            this.kybdMarkTopicRead.BackColor = System.Drawing.SystemColors.Window;
            this.kybdMarkTopicRead.CanBeSelected = false;
            this.kybdMarkTopicRead.CanHaveFocus = true;
            this.kybdMarkTopicRead.Enabled = false;
            this.kybdMarkTopicRead.ExtraData = null;
            this.kybdMarkTopicRead.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdMarkTopicRead.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdMarkTopicRead.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdMarkTopicRead.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdMarkTopicRead.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkTopicRead.ImageScaling = false;
            this.kybdMarkTopicRead.Location = new System.Drawing.Point(499, 298);
            this.kybdMarkTopicRead.Name = "kybdMarkTopicRead";
            this.kybdMarkTopicRead.Size = new System.Drawing.Size(35, 23);
            this.kybdMarkTopicRead.TabIndex = 50;
            this.kybdMarkTopicRead.Text = "Ctrl+K";
            this.kybdMarkTopicRead.UseVisualStyleBackColor = false;
            // 
            // kybdSystemMenuLabel
            // 
            this.kybdSystemMenuLabel.AutoSize = true;
            this.kybdSystemMenuLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdSystemMenuLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdSystemMenuLabel.Location = new System.Drawing.Point(53, 78);
            this.kybdSystemMenuLabel.Name = "kybdSystemMenuLabel";
            this.kybdSystemMenuLabel.Size = new System.Drawing.Size(146, 15);
            this.kybdSystemMenuLabel.TabIndex = 49;
            this.kybdSystemMenuLabel.Text = "Display the system menu";
            // 
            // kybdSystemMenu
            // 
            this.kybdSystemMenu.Active = false;
            this.kybdSystemMenu.BackColor = System.Drawing.SystemColors.Window;
            this.kybdSystemMenu.CanBeSelected = false;
            this.kybdSystemMenu.CanHaveFocus = true;
            this.kybdSystemMenu.Enabled = false;
            this.kybdSystemMenu.ExtraData = null;
            this.kybdSystemMenu.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdSystemMenu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdSystemMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdSystemMenu.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdSystemMenu.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdSystemMenu.ImageScaling = false;
            this.kybdSystemMenu.Location = new System.Drawing.Point(15, 74);
            this.kybdSystemMenu.Name = "kybdSystemMenu";
            this.kybdSystemMenu.Size = new System.Drawing.Size(35, 23);
            this.kybdSystemMenu.TabIndex = 48;
            this.kybdSystemMenu.Text = "F5";
            this.kybdSystemMenu.UseVisualStyleBackColor = false;
            // 
            // kybdHelpLabel
            // 
            this.kybdHelpLabel.AutoSize = true;
            this.kybdHelpLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdHelpLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdHelpLabel.Location = new System.Drawing.Point(53, 46);
            this.kybdHelpLabel.Name = "kybdHelpLabel";
            this.kybdHelpLabel.Size = new System.Drawing.Size(132, 15);
            this.kybdHelpLabel.TabIndex = 41;
            this.kybdHelpLabel.Text = "Display Keyboard Help";
            // 
            // kybdHelp
            // 
            this.kybdHelp.Active = false;
            this.kybdHelp.BackColor = System.Drawing.SystemColors.Window;
            this.kybdHelp.CanBeSelected = false;
            this.kybdHelp.CanHaveFocus = true;
            this.kybdHelp.Enabled = false;
            this.kybdHelp.ExtraData = null;
            this.kybdHelp.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdHelp.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdHelp.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdHelp.ImageScaling = false;
            this.kybdHelp.Location = new System.Drawing.Point(15, 42);
            this.kybdHelp.Name = "kybdHelp";
            this.kybdHelp.Size = new System.Drawing.Size(35, 23);
            this.kybdHelp.TabIndex = 40;
            this.kybdHelp.Text = "F1";
            this.kybdHelp.UseVisualStyleBackColor = false;
            // 
            // kybdSwitchingViewsLabel
            // 
            this.kybdSwitchingViewsLabel.AutoSize = true;
            this.kybdSwitchingViewsLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kybdSwitchingViewsLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdSwitchingViewsLabel.Location = new System.Drawing.Point(12, 13);
            this.kybdSwitchingViewsLabel.Name = "kybdSwitchingViewsLabel";
            this.kybdSwitchingViewsLabel.Size = new System.Drawing.Size(113, 16);
            this.kybdSwitchingViewsLabel.TabIndex = 39;
            this.kybdSwitchingViewsLabel.Text = "Switching Views";
            // 
            // kybdMarkReadLockLabel
            // 
            this.kybdMarkReadLockLabel.AutoSize = true;
            this.kybdMarkReadLockLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdMarkReadLockLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkReadLockLabel.Location = new System.Drawing.Point(536, 110);
            this.kybdMarkReadLockLabel.Name = "kybdMarkReadLockLabel";
            this.kybdMarkReadLockLabel.Size = new System.Drawing.Size(97, 15);
            this.kybdMarkReadLockLabel.TabIndex = 38;
            this.kybdMarkReadLockLabel.Text = "Toggle read lock";
            // 
            // kybdMarkReadLock
            // 
            this.kybdMarkReadLock.Active = false;
            this.kybdMarkReadLock.BackColor = System.Drawing.SystemColors.Window;
            this.kybdMarkReadLock.CanBeSelected = false;
            this.kybdMarkReadLock.CanHaveFocus = true;
            this.kybdMarkReadLock.Enabled = false;
            this.kybdMarkReadLock.ExtraData = null;
            this.kybdMarkReadLock.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdMarkReadLock.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdMarkReadLock.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdMarkReadLock.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdMarkReadLock.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkReadLock.ImageScaling = false;
            this.kybdMarkReadLock.Location = new System.Drawing.Point(498, 106);
            this.kybdMarkReadLock.Name = "kybdMarkReadLock";
            this.kybdMarkReadLock.Size = new System.Drawing.Size(35, 23);
            this.kybdMarkReadLock.TabIndex = 37;
            this.kybdMarkReadLock.Text = "L";
            this.kybdMarkReadLock.UseVisualStyleBackColor = false;
            // 
            // kybdWithdrawLabel
            // 
            this.kybdWithdrawLabel.AutoSize = true;
            this.kybdWithdrawLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdWithdrawLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdWithdrawLabel.Location = new System.Drawing.Point(537, 238);
            this.kybdWithdrawLabel.Name = "kybdWithdrawLabel";
            this.kybdWithdrawLabel.Size = new System.Drawing.Size(114, 15);
            this.kybdWithdrawLabel.TabIndex = 36;
            this.kybdWithdrawLabel.Text = "Withdraw message";
            // 
            // kybdWithdraw
            // 
            this.kybdWithdraw.Active = false;
            this.kybdWithdraw.BackColor = System.Drawing.SystemColors.Window;
            this.kybdWithdraw.CanBeSelected = false;
            this.kybdWithdraw.CanHaveFocus = true;
            this.kybdWithdraw.Enabled = false;
            this.kybdWithdraw.ExtraData = null;
            this.kybdWithdraw.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdWithdraw.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdWithdraw.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdWithdraw.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdWithdraw.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdWithdraw.ImageScaling = false;
            this.kybdWithdraw.Location = new System.Drawing.Point(499, 234);
            this.kybdWithdraw.Name = "kybdWithdraw";
            this.kybdWithdraw.Size = new System.Drawing.Size(35, 23);
            this.kybdWithdraw.TabIndex = 35;
            this.kybdWithdraw.Text = "W";
            this.kybdWithdraw.UseVisualStyleBackColor = false;
            // 
            // kybdGotoLabel
            // 
            this.kybdGotoLabel.AutoSize = true;
            this.kybdGotoLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdGotoLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdGotoLabel.Location = new System.Drawing.Point(300, 238);
            this.kybdGotoLabel.Name = "kybdGotoLabel";
            this.kybdGotoLabel.Size = new System.Drawing.Size(92, 15);
            this.kybdGotoLabel.TabIndex = 36;
            this.kybdGotoLabel.Text = "Go to message";
            // 
            // kybdGoto
            // 
            this.kybdGoto.Active = false;
            this.kybdGoto.BackColor = System.Drawing.SystemColors.Window;
            this.kybdGoto.CanBeSelected = false;
            this.kybdGoto.CanHaveFocus = true;
            this.kybdGoto.Enabled = false;
            this.kybdGoto.ExtraData = null;
            this.kybdGoto.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdGoto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdGoto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdGoto.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdGoto.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdGoto.ImageScaling = false;
            this.kybdGoto.Location = new System.Drawing.Point(262, 234);
            this.kybdGoto.Name = "kybdGoto";
            this.kybdGoto.Size = new System.Drawing.Size(35, 23);
            this.kybdGoto.TabIndex = 35;
            this.kybdGoto.Text = "G";
            this.kybdGoto.UseVisualStyleBackColor = false;
            // 
            // kybdScrollNextUnreadLabel
            // 
            this.kybdScrollNextUnreadLabel.AutoSize = true;
            this.kybdScrollNextUnreadLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdScrollNextUnreadLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdScrollNextUnreadLabel.Location = new System.Drawing.Point(300, 302);
            this.kybdScrollNextUnreadLabel.Name = "kybdScrollNextUnreadLabel";
            this.kybdScrollNextUnreadLabel.Size = new System.Drawing.Size(174, 15);
            this.kybdScrollNextUnreadLabel.TabIndex = 34;
            this.kybdScrollNextUnreadLabel.Text = "Scroll to next unread message";
            // 
            // kybdScrollNextUnread
            // 
            this.kybdScrollNextUnread.Active = false;
            this.kybdScrollNextUnread.BackColor = System.Drawing.SystemColors.Window;
            this.kybdScrollNextUnread.CanBeSelected = false;
            this.kybdScrollNextUnread.CanHaveFocus = true;
            this.kybdScrollNextUnread.Enabled = false;
            this.kybdScrollNextUnread.ExtraData = null;
            this.kybdScrollNextUnread.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdScrollNextUnread.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdScrollNextUnread.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdScrollNextUnread.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdScrollNextUnread.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdScrollNextUnread.ImageScaling = false;
            this.kybdScrollNextUnread.Location = new System.Drawing.Point(262, 298);
            this.kybdScrollNextUnread.Name = "kybdScrollNextUnread";
            this.kybdScrollNextUnread.Size = new System.Drawing.Size(35, 23);
            this.kybdScrollNextUnread.TabIndex = 33;
            this.kybdScrollNextUnread.Text = "Sp";
            this.kybdScrollNextUnread.UseVisualStyleBackColor = false;
            // 
            // kybdAuthoringMessagesLabel
            // 
            this.kybdAuthoringMessagesLabel.AutoSize = true;
            this.kybdAuthoringMessagesLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kybdAuthoringMessagesLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdAuthoringMessagesLabel.Location = new System.Drawing.Point(13, 128);
            this.kybdAuthoringMessagesLabel.Name = "kybdAuthoringMessagesLabel";
            this.kybdAuthoringMessagesLabel.Size = new System.Drawing.Size(135, 16);
            this.kybdAuthoringMessagesLabel.TabIndex = 32;
            this.kybdAuthoringMessagesLabel.Text = "Authoring Messages";
            // 
            // kybdMarkingMessagesLabel
            // 
            this.kybdMarkingMessagesLabel.AutoSize = true;
            this.kybdMarkingMessagesLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kybdMarkingMessagesLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkingMessagesLabel.Location = new System.Drawing.Point(494, 13);
            this.kybdMarkingMessagesLabel.Name = "kybdMarkingMessagesLabel";
            this.kybdMarkingMessagesLabel.Size = new System.Drawing.Size(124, 16);
            this.kybdMarkingMessagesLabel.TabIndex = 31;
            this.kybdMarkingMessagesLabel.Text = "Marking Messages";
            // 
            // kybdDelLabel
            // 
            this.kybdDelLabel.AutoSize = true;
            this.kybdDelLabel.BackColor = System.Drawing.SystemColors.Window;
            this.kybdDelLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdDelLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdDelLabel.Location = new System.Drawing.Point(53, 256);
            this.kybdDelLabel.Name = "kybdDelLabel";
            this.kybdDelLabel.Size = new System.Drawing.Size(136, 15);
            this.kybdDelLabel.TabIndex = 30;
            this.kybdDelLabel.Text = "Delete a draft message";
            // 
            // kybdDel
            // 
            this.kybdDel.Active = false;
            this.kybdDel.BackColor = System.Drawing.SystemColors.Window;
            this.kybdDel.CanBeSelected = false;
            this.kybdDel.CanHaveFocus = true;
            this.kybdDel.Enabled = false;
            this.kybdDel.ExtraData = null;
            this.kybdDel.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdDel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdDel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdDel.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdDel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdDel.ImageScaling = false;
            this.kybdDel.Location = new System.Drawing.Point(15, 252);
            this.kybdDel.Name = "kybdDel";
            this.kybdDel.Size = new System.Drawing.Size(35, 23);
            this.kybdDel.TabIndex = 29;
            this.kybdDel.Text = "Del";
            this.kybdDel.UseVisualStyleBackColor = false;
            // 
            // kybdNextThreadLabel
            // 
            this.kybdNextThreadLabel.AutoSize = true;
            this.kybdNextThreadLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdNextThreadLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdNextThreadLabel.Location = new System.Drawing.Point(300, 142);
            this.kybdNextThreadLabel.Name = "kybdNextThreadLabel";
            this.kybdNextThreadLabel.Size = new System.Drawing.Size(119, 15);
            this.kybdNextThreadLabel.TabIndex = 28;
            this.kybdNextThreadLabel.Text = "Go to the next thread";
            // 
            // kybdNextThread
            // 
            this.kybdNextThread.Active = false;
            this.kybdNextThread.BackColor = System.Drawing.SystemColors.Window;
            this.kybdNextThread.CanBeSelected = false;
            this.kybdNextThread.CanHaveFocus = true;
            this.kybdNextThread.Enabled = false;
            this.kybdNextThread.ExtraData = null;
            this.kybdNextThread.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdNextThread.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdNextThread.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdNextThread.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kybdNextThread.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdNextThread.ImageScaling = false;
            this.kybdNextThread.Location = new System.Drawing.Point(262, 138);
            this.kybdNextThread.Name = "kybdNextThread";
            this.kybdNextThread.Size = new System.Drawing.Size(35, 23);
            this.kybdNextThread.TabIndex = 27;
            this.kybdNextThread.Text = ".";
            this.kybdNextThread.UseVisualStyleBackColor = false;
            // 
            // kybdPreviousThreadLabel
            // 
            this.kybdPreviousThreadLabel.AutoSize = true;
            this.kybdPreviousThreadLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdPreviousThreadLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdPreviousThreadLabel.Location = new System.Drawing.Point(300, 110);
            this.kybdPreviousThreadLabel.Name = "kybdPreviousThreadLabel";
            this.kybdPreviousThreadLabel.Size = new System.Drawing.Size(144, 15);
            this.kybdPreviousThreadLabel.TabIndex = 26;
            this.kybdPreviousThreadLabel.Text = "Go to the previous thread";
            // 
            // kybdPreviousThread
            // 
            this.kybdPreviousThread.Active = false;
            this.kybdPreviousThread.BackColor = System.Drawing.SystemColors.Window;
            this.kybdPreviousThread.CanBeSelected = false;
            this.kybdPreviousThread.CanHaveFocus = true;
            this.kybdPreviousThread.Enabled = false;
            this.kybdPreviousThread.ExtraData = null;
            this.kybdPreviousThread.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdPreviousThread.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdPreviousThread.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdPreviousThread.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kybdPreviousThread.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdPreviousThread.ImageScaling = false;
            this.kybdPreviousThread.Location = new System.Drawing.Point(262, 106);
            this.kybdPreviousThread.Name = "kybdPreviousThread";
            this.kybdPreviousThread.Size = new System.Drawing.Size(35, 23);
            this.kybdPreviousThread.TabIndex = 25;
            this.kybdPreviousThread.Text = ",";
            this.kybdPreviousThread.UseVisualStyleBackColor = false;
            // 
            // kybdMarkStarLabel
            // 
            this.kybdMarkStarLabel.AutoSize = true;
            this.kybdMarkStarLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdMarkStarLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkStarLabel.Location = new System.Drawing.Point(537, 174);
            this.kybdMarkStarLabel.Name = "kybdMarkStarLabel";
            this.kybdMarkStarLabel.Size = new System.Drawing.Size(142, 15);
            this.kybdMarkStarLabel.TabIndex = 24;
            this.kybdMarkStarLabel.Text = "Toggle message starred";
            // 
            // kybdMarkStar
            // 
            this.kybdMarkStar.Active = false;
            this.kybdMarkStar.BackColor = System.Drawing.SystemColors.Window;
            this.kybdMarkStar.CanBeSelected = false;
            this.kybdMarkStar.CanHaveFocus = true;
            this.kybdMarkStar.Enabled = false;
            this.kybdMarkStar.ExtraData = null;
            this.kybdMarkStar.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdMarkStar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdMarkStar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdMarkStar.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdMarkStar.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkStar.ImageScaling = false;
            this.kybdMarkStar.Location = new System.Drawing.Point(499, 170);
            this.kybdMarkStar.Name = "kybdMarkStar";
            this.kybdMarkStar.Size = new System.Drawing.Size(35, 23);
            this.kybdMarkStar.TabIndex = 23;
            this.kybdMarkStar.Text = "8";
            this.kybdMarkStar.UseVisualStyleBackColor = false;
            // 
            // kybdMarkThreadReadLabel
            // 
            this.kybdMarkThreadReadLabel.AutoSize = true;
            this.kybdMarkThreadReadLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdMarkThreadReadLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkThreadReadLabel.Location = new System.Drawing.Point(537, 270);
            this.kybdMarkThreadReadLabel.Name = "kybdMarkThreadReadLabel";
            this.kybdMarkThreadReadLabel.Size = new System.Drawing.Size(119, 15);
            this.kybdMarkThreadReadLabel.TabIndex = 22;
            this.kybdMarkThreadReadLabel.Text = "Mark the thread read";
            // 
            // kybdMarkThreadRead
            // 
            this.kybdMarkThreadRead.Active = false;
            this.kybdMarkThreadRead.BackColor = System.Drawing.SystemColors.Window;
            this.kybdMarkThreadRead.CanBeSelected = false;
            this.kybdMarkThreadRead.CanHaveFocus = true;
            this.kybdMarkThreadRead.Enabled = false;
            this.kybdMarkThreadRead.ExtraData = null;
            this.kybdMarkThreadRead.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdMarkThreadRead.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdMarkThreadRead.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdMarkThreadRead.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdMarkThreadRead.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkThreadRead.ImageScaling = false;
            this.kybdMarkThreadRead.Location = new System.Drawing.Point(499, 266);
            this.kybdMarkThreadRead.Name = "kybdMarkThreadRead";
            this.kybdMarkThreadRead.Size = new System.Drawing.Size(35, 23);
            this.kybdMarkThreadRead.TabIndex = 21;
            this.kybdMarkThreadRead.Text = "Z";
            this.kybdMarkThreadRead.UseVisualStyleBackColor = false;
            // 
            // kybdExpandThreadLabel
            // 
            this.kybdExpandThreadLabel.AutoSize = true;
            this.kybdExpandThreadLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdExpandThreadLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdExpandThreadLabel.Location = new System.Drawing.Point(300, 270);
            this.kybdExpandThreadLabel.Name = "kybdExpandThreadLabel";
            this.kybdExpandThreadLabel.Size = new System.Drawing.Size(170, 15);
            this.kybdExpandThreadLabel.TabIndex = 20;
            this.kybdExpandThreadLabel.Text = "Expand and collapse a thread";
            // 
            // kybdExpandThread
            // 
            this.kybdExpandThread.Active = false;
            this.kybdExpandThread.BackColor = System.Drawing.SystemColors.Window;
            this.kybdExpandThread.CanBeSelected = false;
            this.kybdExpandThread.CanHaveFocus = true;
            this.kybdExpandThread.Enabled = false;
            this.kybdExpandThread.ExtraData = null;
            this.kybdExpandThread.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdExpandThread.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdExpandThread.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdExpandThread.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdExpandThread.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdExpandThread.ImageScaling = false;
            this.kybdExpandThread.Location = new System.Drawing.Point(262, 266);
            this.kybdExpandThread.Name = "kybdExpandThread";
            this.kybdExpandThread.Size = new System.Drawing.Size(35, 23);
            this.kybdExpandThread.TabIndex = 19;
            this.kybdExpandThread.Text = "X";
            this.kybdExpandThread.UseVisualStyleBackColor = false;
            // 
            // kybdMarkPriorityLabel
            // 
            this.kybdMarkPriorityLabel.AutoSize = true;
            this.kybdMarkPriorityLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdMarkPriorityLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkPriorityLabel.Location = new System.Drawing.Point(536, 142);
            this.kybdMarkPriorityLabel.Name = "kybdMarkPriorityLabel";
            this.kybdMarkPriorityLabel.Size = new System.Drawing.Size(139, 15);
            this.kybdMarkPriorityLabel.TabIndex = 18;
            this.kybdMarkPriorityLabel.Text = "Toggle message priority";
            // 
            // kybdMarkPriority
            // 
            this.kybdMarkPriority.Active = false;
            this.kybdMarkPriority.BackColor = System.Drawing.SystemColors.Window;
            this.kybdMarkPriority.CanBeSelected = false;
            this.kybdMarkPriority.CanHaveFocus = true;
            this.kybdMarkPriority.Enabled = false;
            this.kybdMarkPriority.ExtraData = null;
            this.kybdMarkPriority.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdMarkPriority.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdMarkPriority.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdMarkPriority.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdMarkPriority.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkPriority.ImageScaling = false;
            this.kybdMarkPriority.Location = new System.Drawing.Point(498, 138);
            this.kybdMarkPriority.Name = "kybdMarkPriority";
            this.kybdMarkPriority.Size = new System.Drawing.Size(35, 23);
            this.kybdMarkPriority.TabIndex = 17;
            this.kybdMarkPriority.Text = "H";
            this.kybdMarkPriority.UseVisualStyleBackColor = false;
            // 
            // kybdOriginalLabel
            // 
            this.kybdOriginalLabel.AutoSize = true;
            this.kybdOriginalLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdOriginalLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdOriginalLabel.Location = new System.Drawing.Point(300, 174);
            this.kybdOriginalLabel.Name = "kybdOriginalLabel";
            this.kybdOriginalLabel.Size = new System.Drawing.Size(150, 15);
            this.kybdOriginalLabel.TabIndex = 16;
            this.kybdOriginalLabel.Text = "Go to the parent message";
            // 
            // kybdOriginal
            // 
            this.kybdOriginal.Active = false;
            this.kybdOriginal.BackColor = System.Drawing.SystemColors.Window;
            this.kybdOriginal.CanBeSelected = false;
            this.kybdOriginal.CanHaveFocus = true;
            this.kybdOriginal.Enabled = false;
            this.kybdOriginal.ExtraData = null;
            this.kybdOriginal.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdOriginal.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdOriginal.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdOriginal.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdOriginal.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdOriginal.ImageScaling = false;
            this.kybdOriginal.Location = new System.Drawing.Point(262, 170);
            this.kybdOriginal.Name = "kybdOriginal";
            this.kybdOriginal.Size = new System.Drawing.Size(35, 23);
            this.kybdOriginal.TabIndex = 15;
            this.kybdOriginal.Text = "O";
            this.kybdOriginal.UseVisualStyleBackColor = false;
            // 
            // kybdCommentLabel
            // 
            this.kybdCommentLabel.AutoSize = true;
            this.kybdCommentLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdCommentLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdCommentLabel.Location = new System.Drawing.Point(53, 192);
            this.kybdCommentLabel.Name = "kybdCommentLabel";
            this.kybdCommentLabel.Size = new System.Drawing.Size(177, 15);
            this.kybdCommentLabel.TabIndex = 14;
            this.kybdCommentLabel.Text = "Reply to the selected message";
            // 
            // kybdComment
            // 
            this.kybdComment.Active = false;
            this.kybdComment.BackColor = System.Drawing.SystemColors.Window;
            this.kybdComment.CanBeSelected = false;
            this.kybdComment.CanHaveFocus = true;
            this.kybdComment.Enabled = false;
            this.kybdComment.ExtraData = null;
            this.kybdComment.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdComment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdComment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdComment.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdComment.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdComment.ImageScaling = false;
            this.kybdComment.Location = new System.Drawing.Point(15, 188);
            this.kybdComment.Name = "kybdComment";
            this.kybdComment.Size = new System.Drawing.Size(35, 23);
            this.kybdComment.TabIndex = 13;
            this.kybdComment.Text = "C";
            this.kybdComment.UseVisualStyleBackColor = false;
            // 
            // kybdSayLabel
            // 
            this.kybdSayLabel.AutoSize = true;
            this.kybdSayLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdSayLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdSayLabel.Location = new System.Drawing.Point(53, 160);
            this.kybdSayLabel.Name = "kybdSayLabel";
            this.kybdSayLabel.Size = new System.Drawing.Size(106, 15);
            this.kybdSayLabel.TabIndex = 12;
            this.kybdSayLabel.Text = "Start a new thread";
            // 
            // kybdSay
            // 
            this.kybdSay.Active = false;
            this.kybdSay.BackColor = System.Drawing.SystemColors.Window;
            this.kybdSay.CanBeSelected = false;
            this.kybdSay.CanHaveFocus = true;
            this.kybdSay.Enabled = false;
            this.kybdSay.ExtraData = null;
            this.kybdSay.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdSay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdSay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdSay.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdSay.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdSay.ImageScaling = false;
            this.kybdSay.Location = new System.Drawing.Point(15, 156);
            this.kybdSay.Name = "kybdSay";
            this.kybdSay.Size = new System.Drawing.Size(35, 23);
            this.kybdSay.TabIndex = 11;
            this.kybdSay.Text = "S";
            this.kybdSay.UseVisualStyleBackColor = false;
            // 
            // kybdNextPriorityLabel
            // 
            this.kybdNextPriorityLabel.AutoSize = true;
            this.kybdNextPriorityLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdNextPriorityLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdNextPriorityLabel.Location = new System.Drawing.Point(300, 206);
            this.kybdNextPriorityLabel.Name = "kybdNextPriorityLabel";
            this.kybdNextPriorityLabel.Size = new System.Drawing.Size(168, 15);
            this.kybdNextPriorityLabel.TabIndex = 10;
            this.kybdNextPriorityLabel.Text = "Next unread priority message";
            // 
            // kybdNextPriority
            // 
            this.kybdNextPriority.Active = false;
            this.kybdNextPriority.BackColor = System.Drawing.SystemColors.Window;
            this.kybdNextPriority.CanBeSelected = false;
            this.kybdNextPriority.CanHaveFocus = true;
            this.kybdNextPriority.Enabled = false;
            this.kybdNextPriority.ExtraData = null;
            this.kybdNextPriority.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdNextPriority.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdNextPriority.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdNextPriority.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdNextPriority.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdNextPriority.ImageScaling = false;
            this.kybdNextPriority.Location = new System.Drawing.Point(262, 202);
            this.kybdNextPriority.Name = "kybdNextPriority";
            this.kybdNextPriority.Size = new System.Drawing.Size(35, 23);
            this.kybdNextPriority.TabIndex = 9;
            this.kybdNextPriority.Text = "P";
            this.kybdNextPriority.UseVisualStyleBackColor = false;
            // 
            // kybdMarkIgnoreLabel
            // 
            this.kybdMarkIgnoreLabel.AutoSize = true;
            this.kybdMarkIgnoreLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdMarkIgnoreLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkIgnoreLabel.Location = new System.Drawing.Point(536, 78);
            this.kybdMarkIgnoreLabel.Name = "kybdMarkIgnoreLabel";
            this.kybdMarkIgnoreLabel.Size = new System.Drawing.Size(138, 15);
            this.kybdMarkIgnoreLabel.TabIndex = 8;
            this.kybdMarkIgnoreLabel.Text = "Toggle message ignore";
            // 
            // kybdMarkIgnore
            // 
            this.kybdMarkIgnore.Active = false;
            this.kybdMarkIgnore.BackColor = System.Drawing.SystemColors.Window;
            this.kybdMarkIgnore.CanBeSelected = false;
            this.kybdMarkIgnore.CanHaveFocus = true;
            this.kybdMarkIgnore.Enabled = false;
            this.kybdMarkIgnore.ExtraData = null;
            this.kybdMarkIgnore.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdMarkIgnore.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdMarkIgnore.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdMarkIgnore.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdMarkIgnore.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkIgnore.ImageScaling = false;
            this.kybdMarkIgnore.Location = new System.Drawing.Point(498, 74);
            this.kybdMarkIgnore.Name = "kybdMarkIgnore";
            this.kybdMarkIgnore.Size = new System.Drawing.Size(35, 23);
            this.kybdMarkIgnore.TabIndex = 7;
            this.kybdMarkIgnore.Text = "I";
            this.kybdMarkIgnore.UseVisualStyleBackColor = false;
            // 
            // kybdMarkReadLabel
            // 
            this.kybdMarkReadLabel.AutoSize = true;
            this.kybdMarkReadLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdMarkReadLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkReadLabel.Location = new System.Drawing.Point(536, 46);
            this.kybdMarkReadLabel.Name = "kybdMarkReadLabel";
            this.kybdMarkReadLabel.Size = new System.Drawing.Size(128, 15);
            this.kybdMarkReadLabel.TabIndex = 6;
            this.kybdMarkReadLabel.Text = "Toggle message read";
            // 
            // kybdMarkRead
            // 
            this.kybdMarkRead.Active = false;
            this.kybdMarkRead.BackColor = System.Drawing.SystemColors.Window;
            this.kybdMarkRead.CanBeSelected = false;
            this.kybdMarkRead.CanHaveFocus = true;
            this.kybdMarkRead.Enabled = false;
            this.kybdMarkRead.ExtraData = null;
            this.kybdMarkRead.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdMarkRead.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdMarkRead.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdMarkRead.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdMarkRead.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdMarkRead.ImageScaling = false;
            this.kybdMarkRead.Location = new System.Drawing.Point(498, 42);
            this.kybdMarkRead.Name = "kybdMarkRead";
            this.kybdMarkRead.Size = new System.Drawing.Size(35, 23);
            this.kybdMarkRead.TabIndex = 5;
            this.kybdMarkRead.Text = "R";
            this.kybdMarkRead.UseVisualStyleBackColor = false;
            // 
            // kybdPreviousLabel
            // 
            this.kybdPreviousLabel.AutoSize = true;
            this.kybdPreviousLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdPreviousLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdPreviousLabel.Location = new System.Drawing.Point(300, 78);
            this.kybdPreviousLabel.Name = "kybdPreviousLabel";
            this.kybdPreviousLabel.Size = new System.Drawing.Size(153, 15);
            this.kybdPreviousLabel.TabIndex = 4;
            this.kybdPreviousLabel.Text = "Back to previous message";
            // 
            // kybdPrevious
            // 
            this.kybdPrevious.Active = false;
            this.kybdPrevious.BackColor = System.Drawing.SystemColors.Window;
            this.kybdPrevious.CanBeSelected = false;
            this.kybdPrevious.CanHaveFocus = true;
            this.kybdPrevious.Enabled = false;
            this.kybdPrevious.ExtraData = null;
            this.kybdPrevious.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdPrevious.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdPrevious.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdPrevious.Font = new System.Drawing.Font("Lucida Console", 11.25F);
            this.kybdPrevious.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdPrevious.ImageScaling = false;
            this.kybdPrevious.Location = new System.Drawing.Point(262, 74);
            this.kybdPrevious.Name = "kybdPrevious";
            this.kybdPrevious.Size = new System.Drawing.Size(35, 23);
            this.kybdPrevious.TabIndex = 3;
            this.kybdPrevious.Text = "←";
            this.kybdPrevious.UseVisualStyleBackColor = false;
            // 
            // kybdNextUnreadLabel
            // 
            this.kybdNextUnreadLabel.AutoSize = true;
            this.kybdNextUnreadLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdNextUnreadLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdNextUnreadLabel.Location = new System.Drawing.Point(300, 46);
            this.kybdNextUnreadLabel.Name = "kybdNextUnreadLabel";
            this.kybdNextUnreadLabel.Size = new System.Drawing.Size(129, 15);
            this.kybdNextUnreadLabel.TabIndex = 2;
            this.kybdNextUnreadLabel.Text = "Next unread message";
            // 
            // kybdNextUnread
            // 
            this.kybdNextUnread.Active = false;
            this.kybdNextUnread.BackColor = System.Drawing.SystemColors.Window;
            this.kybdNextUnread.CanBeSelected = false;
            this.kybdNextUnread.CanHaveFocus = true;
            this.kybdNextUnread.Enabled = false;
            this.kybdNextUnread.ExtraData = null;
            this.kybdNextUnread.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdNextUnread.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdNextUnread.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdNextUnread.Font = new System.Drawing.Font("Lucida Console", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kybdNextUnread.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdNextUnread.ImageScaling = false;
            this.kybdNextUnread.Location = new System.Drawing.Point(262, 42);
            this.kybdNextUnread.Name = "kybdNextUnread";
            this.kybdNextUnread.Size = new System.Drawing.Size(35, 23);
            this.kybdNextUnread.TabIndex = 1;
            this.kybdNextUnread.Text = "→";
            this.kybdNextUnread.UseVisualStyleBackColor = false;
            // 
            // kybdNavigatingMessagesLabel
            // 
            this.kybdNavigatingMessagesLabel.AutoSize = true;
            this.kybdNavigatingMessagesLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kybdNavigatingMessagesLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdNavigatingMessagesLabel.Location = new System.Drawing.Point(259, 13);
            this.kybdNavigatingMessagesLabel.Name = "kybdNavigatingMessagesLabel";
            this.kybdNavigatingMessagesLabel.Size = new System.Drawing.Size(141, 16);
            this.kybdNavigatingMessagesLabel.TabIndex = 0;
            this.kybdNavigatingMessagesLabel.Text = "Navigating Messages";
            // 
            // kybdQuoteLabel
            // 
            this.kybdQuoteLabel.AutoSize = true;
            this.kybdQuoteLabel.Font = new System.Drawing.Font("Arial", 9F);
            this.kybdQuoteLabel.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdQuoteLabel.Location = new System.Drawing.Point(53, 224);
            this.kybdQuoteLabel.Name = "kybdQuoteLabel";
            this.kybdQuoteLabel.Size = new System.Drawing.Size(96, 15);
            this.kybdQuoteLabel.TabIndex = 57;
            this.kybdQuoteLabel.Text = "Reply and quote";
            // 
            // kybdQuote
            // 
            this.kybdQuote.Active = false;
            this.kybdQuote.BackColor = System.Drawing.SystemColors.Window;
            this.kybdQuote.CanBeSelected = false;
            this.kybdQuote.CanHaveFocus = true;
            this.kybdQuote.Enabled = false;
            this.kybdQuote.ExtraData = null;
            this.kybdQuote.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.kybdQuote.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.kybdQuote.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightGray;
            this.kybdQuote.Font = new System.Drawing.Font("Arial", 8F);
            this.kybdQuote.ForeColor = System.Drawing.SystemColors.WindowText;
            this.kybdQuote.ImageScaling = false;
            this.kybdQuote.Location = new System.Drawing.Point(15, 220);
            this.kybdQuote.Name = "kybdQuote";
            this.kybdQuote.Size = new System.Drawing.Size(35, 23);
            this.kybdQuote.TabIndex = 56;
            this.kybdQuote.Text = "Q";
            this.kybdQuote.UseVisualStyleBackColor = false;
            // 
            // KeyboardHelp
            // 
            this.AcceptButton = this.kybdClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.kybdClose;
            this.ClientSize = new System.Drawing.Size(701, 408);
            this.Controls.Add(this.kybdClose);
            this.Controls.Add(this.kybdPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "KeyboardHelp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Keyboard Help";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KeyboardHelp_FormClosing);
            this.Load += new System.EventHandler(this.KeyboardHelp_Load);
            this.kybdPanel.ResumeLayout(false);
            this.kybdPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CRPanel kybdPanel;
        private System.Windows.Forms.Button kybdClose;
        private System.Windows.Forms.Label kybdCommentLabel;
        private Controls.CRRoundButton kybdComment;
        private System.Windows.Forms.Label kybdSayLabel;
        private Controls.CRRoundButton kybdSay;
        private System.Windows.Forms.Label kybdNextPriorityLabel;
        private Controls.CRRoundButton kybdNextPriority;
        private System.Windows.Forms.Label kybdMarkIgnoreLabel;
        private Controls.CRRoundButton kybdMarkIgnore;
        private System.Windows.Forms.Label kybdMarkReadLabel;
        private Controls.CRRoundButton kybdMarkRead;
        private System.Windows.Forms.Label kybdPreviousLabel;
        private Controls.CRRoundButton kybdPrevious;
        private System.Windows.Forms.Label kybdNextUnreadLabel;
        private Controls.CRRoundButton kybdNextUnread;
        private System.Windows.Forms.Label kybdNavigatingMessagesLabel;
        private System.Windows.Forms.Label kybdOriginalLabel;
        private Controls.CRRoundButton kybdOriginal;
        private System.Windows.Forms.Label kybdMarkPriorityLabel;
        private Controls.CRRoundButton kybdMarkPriority;
        private System.Windows.Forms.Label kybdMarkThreadReadLabel;
        private Controls.CRRoundButton kybdMarkThreadRead;
        private System.Windows.Forms.Label kybdExpandThreadLabel;
        private Controls.CRRoundButton kybdExpandThread;
        private System.Windows.Forms.Label kybdMarkStarLabel;
        private Controls.CRRoundButton kybdMarkStar;
        private System.Windows.Forms.Label kybdNextThreadLabel;
        private Controls.CRRoundButton kybdNextThread;
        private System.Windows.Forms.Label kybdPreviousThreadLabel;
        private Controls.CRRoundButton kybdPreviousThread;
        private System.Windows.Forms.Label kybdDelLabel;
        private Controls.CRRoundButton kybdDel;
        private System.Windows.Forms.Label kybdAuthoringMessagesLabel;
        private System.Windows.Forms.Label kybdMarkingMessagesLabel;
        private System.Windows.Forms.Label kybdScrollNextUnreadLabel;
        private CRRoundButton kybdScrollNextUnread;
        private System.Windows.Forms.Label kybdWithdrawLabel;
        private Controls.CRRoundButton kybdWithdraw;
        private System.Windows.Forms.Label kybdGotoLabel;
        private Controls.CRRoundButton kybdGoto;
        private System.Windows.Forms.Label kybdMarkReadLockLabel;
        private CRRoundButton kybdMarkReadLock;
        private System.Windows.Forms.Label kybdSystemMenuLabel;
        private CRRoundButton kybdSystemMenu;
        private System.Windows.Forms.Label kybdHelpLabel;
        private CRRoundButton kybdHelp;
        private System.Windows.Forms.Label kybdSwitchingViewsLabel;
        private System.Windows.Forms.Label kybdMarkTopicReadLabel;
        private CRRoundButton kybdMarkTopicRead;
        private System.Windows.Forms.Label kybdSearchLabel;
        private CRRoundButton kybdSearch;
        private System.Windows.Forms.Label kybdTogglePlainTextLabel;
        private CRRoundButton kybdTogglePlainText;
        private System.Windows.Forms.Label kybdQuoteLabel;
        private CRRoundButton kybdQuote;
    }
}