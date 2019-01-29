using System.ComponentModel;
using System.Windows.Forms;
using CIXReader.Controls;

namespace CIXReader.Forms
{
    sealed partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            #if !__MonoCS__
            if (disposing && (_sparkle != null))
            {
            _sparkle.Dispose();
            }
            #endif
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainPopupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mainAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mainKeyboardHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mainSupport = new System.Windows.Forms.ToolStripMenuItem();
            this.mainDiagnostics = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mainCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.mainViewChangeLog = new System.Windows.Forms.ToolStripMenuItem();
            this.mainInstallScript = new System.Windows.Forms.ToolStripMenuItem();
            this.mainSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mainOffline = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mainLogoff = new System.Windows.Forms.ToolStripMenuItem();
            this.mainExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenubar = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileNewPrivateMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileReply = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileReplyByMail = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileOffline = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFilePageSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFilePrint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileSignOut = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditCopyLink = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuEditQuote = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuCIXReaderSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCIXReaderAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewSortBy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.menuViewGroupByConv = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewCollapseConv = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.menuViewProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.menuViewShowPlainText = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewShowInlineImages = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewShowIgnoredMessages = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuViewPreviousMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewNextUnread = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewNextUnreadPriority = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
            this.menuViewPreviousRoot = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewNextRoot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuViewAllTopics = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewRecentTopics = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
            this.menuViewShowToolBar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewCustomiseToolbar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.menuViewShowStatusBar = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMessageNewMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMessageReply = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMessageEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.markToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMessageMarkUnread = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMessageMarkPriority = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMessageMarkIgnored = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMessageMarkReadLock = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMessageMarkStarred = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMessageMarkThreadRead = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMessageGoTo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMessageGoToOriginal = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMessageWithdraw = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMessageBlock = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFolderJoinForum = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFolderDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFolderResign = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFolderManage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFolderParticipants = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFolderMarkAllRead = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFolderRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.menuHelpDiagnostics = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpCIXSupport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpViewChangeLog = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelpKeyboardHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.menuCIXReaderAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewSortByDate = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewSortByAuthor = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewSortBySubject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuViewSortAscendingForTopic = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewSortDescendingForTopic = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewSortByTopic = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuViewSortByDirectory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuViewSortByName = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewSortByPopularity = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewSortByTitle = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewSortBySubCategory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuViewSortAscendingForDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewSortDescendingForDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.mainStatusBar = new CIXReader.Controls.CRPanel();
            this.mainStatusText = new System.Windows.Forms.Label();
            this.mainProgress = new CIXReader.Controls.CRProgress();
            this.refreshtCtrlTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainPopupMenu.SuspendLayout();
            this.mainMenubar.SuspendLayout();
            this.menuViewSortByTopic.SuspendLayout();
            this.menuViewSortByDirectory.SuspendLayout();
            this.mainStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPopupMenu
            // 
            this.mainPopupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainAbout,
            this.toolStripSeparator4,
            this.mainKeyboardHelp,
            this.mainSupport,
            this.mainDiagnostics,
            this.toolStripSeparator3,
            this.mainCheckForUpdates,
            this.mainViewChangeLog,
            this.mainInstallScript,
            this.mainSettings,
            this.toolStripSeparator1,
            this.mainOffline,
            this.toolStripSeparator2,
            this.mainLogoff,
            this.mainExit});
            this.mainPopupMenu.Name = "mainPopupMenu";
            this.mainPopupMenu.Size = new System.Drawing.Size(183, 270);
            // 
            // mainAbout
            // 
            this.mainAbout.Name = "mainAbout";
            this.mainAbout.Size = new System.Drawing.Size(182, 22);
            this.mainAbout.Text = "About...";
            this.mainAbout.Click += new System.EventHandler(this.mainAbout_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(179, 6);
            // 
            // mainKeyboardHelp
            // 
            this.mainKeyboardHelp.Name = "mainKeyboardHelp";
            this.mainKeyboardHelp.Size = new System.Drawing.Size(182, 22);
            this.mainKeyboardHelp.Text = "Keyboard Help";
            this.mainKeyboardHelp.Click += new System.EventHandler(this.mainKeyboardHelp_Click);
            // 
            // mainSupport
            // 
            this.mainSupport.Name = "mainSupport";
            this.mainSupport.Size = new System.Drawing.Size(182, 22);
            this.mainSupport.Text = "CIX Support";
            this.mainSupport.Click += new System.EventHandler(this.mainSupport_Click);
            // 
            // mainDiagnostics
            // 
            this.mainDiagnostics.Name = "mainDiagnostics";
            this.mainDiagnostics.Size = new System.Drawing.Size(182, 22);
            this.mainDiagnostics.Text = "Diagnostics Info";
            this.mainDiagnostics.Click += new System.EventHandler(this.mainDiagnostics_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(179, 6);
            // 
            // mainCheckForUpdates
            // 
            this.mainCheckForUpdates.Name = "mainCheckForUpdates";
            this.mainCheckForUpdates.Size = new System.Drawing.Size(182, 22);
            this.mainCheckForUpdates.Text = "Check For Updates...";
            this.mainCheckForUpdates.Click += new System.EventHandler(this.mainCheckForUpdates_Click);
            // 
            // mainViewChangeLog
            // 
            this.mainViewChangeLog.Name = "mainViewChangeLog";
            this.mainViewChangeLog.Size = new System.Drawing.Size(182, 22);
            this.mainViewChangeLog.Text = "View Change Log";
            this.mainViewChangeLog.Click += new System.EventHandler(this.menuHelpViewChangeLog_Click);
            // 
            // mainInstallScript
            // 
            this.mainInstallScript.Name = "mainInstallScript";
            this.mainInstallScript.Size = new System.Drawing.Size(182, 22);
            this.mainInstallScript.Text = "Install Script...";
            this.mainInstallScript.Click += new System.EventHandler(this.mainInstallScript_Click);
            // 
            // mainSettings
            // 
            this.mainSettings.Name = "mainSettings";
            this.mainSettings.Size = new System.Drawing.Size(182, 22);
            this.mainSettings.Text = "Settings...";
            this.mainSettings.Click += new System.EventHandler(this.mainSettings_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(179, 6);
            // 
            // mainOffline
            // 
            this.mainOffline.Name = "mainOffline";
            this.mainOffline.Size = new System.Drawing.Size(182, 22);
            this.mainOffline.Text = "Offline";
            this.mainOffline.Click += new System.EventHandler(this.mainOffline_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(179, 6);
            // 
            // mainLogoff
            // 
            this.mainLogoff.Name = "mainLogoff";
            this.mainLogoff.Size = new System.Drawing.Size(182, 22);
            this.mainLogoff.Text = "Log Off";
            this.mainLogoff.Click += new System.EventHandler(this.mainLogoff_Click);
            // 
            // mainExit
            // 
            this.mainExit.Name = "mainExit";
            this.mainExit.Size = new System.Drawing.Size(182, 22);
            this.mainExit.Text = "Exit";
            this.mainExit.Click += new System.EventHandler(this.mainExit_Click);
            // 
            // mainMenubar
            // 
            this.mainMenubar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuView,
            this.menuMessage,
            this.menuFolder,
            this.menuHelp});
            this.mainMenubar.Location = new System.Drawing.Point(0, 0);
            this.mainMenubar.Name = "mainMenubar";
            this.mainMenubar.Size = new System.Drawing.Size(895, 24);
            this.mainMenubar.TabIndex = 2;
            this.mainMenubar.Text = "menuStrip1";
            this.mainMenubar.MenuActivate += new System.EventHandler(this.mainMenubar_MenuActivate);
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileNewPrivateMessage,
            this.menuFileReply,
            this.menuFileReplyByMail,
            this.toolStripMenuItem1,
            this.menuFileOffline,
            this.menuFileRefresh,
            this.toolStripMenuItem3,
            this.menuFilePageSetup,
            this.menuFilePrint,
            this.toolStripMenuItem4,
            this.menuFileSignOut,
            this.toolStripMenuItem7,
            this.menuFileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 20);
            this.menuFile.Text = "&File";
            // 
            // menuFileNewPrivateMessage
            // 
            this.menuFileNewPrivateMessage.Name = "menuFileNewPrivateMessage";
            this.menuFileNewPrivateMessage.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
            this.menuFileNewPrivateMessage.Size = new System.Drawing.Size(229, 22);
            this.menuFileNewPrivateMessage.Text = "&New PMessage";
            this.menuFileNewPrivateMessage.Click += new System.EventHandler(this.menuFileNewPrivateMessage_Click);
            // 
            // menuFileReply
            // 
            this.menuFileReply.Name = "menuFileReply";
            this.menuFileReply.Size = new System.Drawing.Size(229, 22);
            this.menuFileReply.Text = "&Reply...";
            this.menuFileReply.Click += new System.EventHandler(this.menuFileReply_Click);
            // 
            // menuFileReplyByMail
            // 
            this.menuFileReplyByMail.Name = "menuFileReplyByMail";
            this.menuFileReplyByMail.Size = new System.Drawing.Size(229, 22);
            this.menuFileReplyByMail.Text = "Reply B&y PMessage";
            this.menuFileReplyByMail.Click += new System.EventHandler(this.menuFileReplyByMail_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(226, 6);
            // 
            // menuFileOffline
            // 
            this.menuFileOffline.Name = "menuFileOffline";
            this.menuFileOffline.Size = new System.Drawing.Size(229, 22);
            this.menuFileOffline.Text = "&Offline";
            this.menuFileOffline.Click += new System.EventHandler(this.mainOffline_Click);
            // 
            // menuFileRefresh
            // 
            this.menuFileRefresh.Name = "menuFileRefresh";
            this.menuFileRefresh.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.menuFileRefresh.Size = new System.Drawing.Size(229, 22);
            this.menuFileRefresh.Text = "&Refresh";
            this.menuFileRefresh.Click += new System.EventHandler(this.menuFileRefresh_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(226, 6);
            // 
            // menuFilePageSetup
            // 
            this.menuFilePageSetup.Name = "menuFilePageSetup";
            this.menuFilePageSetup.Size = new System.Drawing.Size(229, 22);
            this.menuFilePageSetup.Text = "Page &Setup...";
            this.menuFilePageSetup.Click += new System.EventHandler(this.menuFilePageSetup_Click);
            // 
            // menuFilePrint
            // 
            this.menuFilePrint.Name = "menuFilePrint";
            this.menuFilePrint.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.menuFilePrint.Size = new System.Drawing.Size(229, 22);
            this.menuFilePrint.Text = "&Print...";
            this.menuFilePrint.Click += new System.EventHandler(this.menuFilePrint_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(226, 6);
            // 
            // menuFileSignOut
            // 
            this.menuFileSignOut.Name = "menuFileSignOut";
            this.menuFileSignOut.Size = new System.Drawing.Size(229, 22);
            this.menuFileSignOut.Text = "Si&gn Out...";
            this.menuFileSignOut.Click += new System.EventHandler(this.mainLogoff_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(226, 6);
            // 
            // menuFileExit
            // 
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuFileExit.Size = new System.Drawing.Size(229, 22);
            this.menuFileExit.Text = "E&xit";
            this.menuFileExit.Click += new System.EventHandler(this.mainExit_Click);
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEditCopy,
            this.menuEditCopyLink,
            this.menuEditSelectAll,
            this.toolStripMenuItem2,
            this.menuEditQuote,
            this.toolStripSeparator5,
            this.menuCIXReaderSettings,
            this.menuCIXReaderAccount});
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(39, 20);
            this.menuEdit.Text = "&Edit";
            // 
            // menuEditCopy
            // 
            this.menuEditCopy.Name = "menuEditCopy";
            this.menuEditCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.menuEditCopy.Size = new System.Drawing.Size(201, 22);
            this.menuEditCopy.Text = "&Copy";
            this.menuEditCopy.Click += new System.EventHandler(this.menuEditCopy_Click);
            // 
            // menuEditCopyLink
            // 
            this.menuEditCopyLink.Name = "menuEditCopyLink";
            this.menuEditCopyLink.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.menuEditCopyLink.Size = new System.Drawing.Size(201, 22);
            this.menuEditCopyLink.Text = "Copy &Link";
            this.menuEditCopyLink.Click += new System.EventHandler(this.menuEditCopyLink_Click);
            // 
            // menuEditSelectAll
            // 
            this.menuEditSelectAll.Name = "menuEditSelectAll";
            this.menuEditSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.menuEditSelectAll.Size = new System.Drawing.Size(201, 22);
            this.menuEditSelectAll.Text = "Select &All";
            this.menuEditSelectAll.Click += new System.EventHandler(this.menuEditSelectAll_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(198, 6);
            // 
            // menuEditQuote
            // 
            this.menuEditQuote.Name = "menuEditQuote";
            this.menuEditQuote.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Q)));
            this.menuEditQuote.Size = new System.Drawing.Size(201, 22);
            this.menuEditQuote.Text = "&Quote...";
            this.menuEditQuote.Click += new System.EventHandler(this.menuEditQuote_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(198, 6);
            // 
            // menuCIXReaderSettings
            // 
            this.menuCIXReaderSettings.Name = "menuCIXReaderSettings";
            this.menuCIXReaderSettings.Size = new System.Drawing.Size(201, 22);
            this.menuCIXReaderSettings.Text = "&Settings...";
            this.menuCIXReaderSettings.Click += new System.EventHandler(this.mainSettings_Click);
            // 
            // menuCIXReaderAccount
            // 
            this.menuCIXReaderAccount.Name = "menuCIXReaderAccount";
            this.menuCIXReaderAccount.Size = new System.Drawing.Size(201, 22);
            this.menuCIXReaderAccount.Text = "Accoun&t...";
            this.menuCIXReaderAccount.Click += new System.EventHandler(this.menuCIXReaderAccount_Click);
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewSortBy,
            this.toolStripSeparator12,
            this.menuViewGroupByConv,
            this.menuViewCollapseConv,
            this.toolStripSeparator10,
            this.menuViewProfile,
            this.toolStripMenuItem8,
            this.menuViewShowPlainText,
            this.menuViewShowInlineImages,
            this.menuViewShowIgnoredMessages,
            this.toolStripMenuItem9,
            this.menuViewPreviousMessage,
            this.menuViewNextUnread,
            this.menuViewNextUnreadPriority,
            this.toolStripMenuItem10,
            this.menuViewPreviousRoot,
            this.menuViewNextRoot,
            this.toolStripMenuItem5,
            this.menuViewAllTopics,
            this.menuViewRecentTopics,
            this.toolStripMenuItem11,
            this.menuViewShowToolBar,
            this.menuViewCustomiseToolbar,
            this.toolStripSeparator11,
            this.menuViewShowStatusBar});
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size(44, 20);
            this.menuView.Text = "&View";
            // 
            // menuViewSortBy
            // 
            this.menuViewSortBy.Name = "menuViewSortBy";
            this.menuViewSortBy.Size = new System.Drawing.Size(201, 22);
            this.menuViewSortBy.Text = "&Sort By";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(198, 6);
            // 
            // menuViewGroupByConv
            // 
            this.menuViewGroupByConv.Name = "menuViewGroupByConv";
            this.menuViewGroupByConv.Size = new System.Drawing.Size(201, 22);
            this.menuViewGroupByConv.Text = "G&roup By Conversations";
            this.menuViewGroupByConv.Click += new System.EventHandler(this.menuViewGroupByConv_Click);
            // 
            // menuViewCollapseConv
            // 
            this.menuViewCollapseConv.Name = "menuViewCollapseConv";
            this.menuViewCollapseConv.Size = new System.Drawing.Size(201, 22);
            this.menuViewCollapseConv.Text = "Collapse Con&versations";
            this.menuViewCollapseConv.Click += new System.EventHandler(this.menuViewCollapseConv_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(198, 6);
            // 
            // menuViewProfile
            // 
            this.menuViewProfile.Name = "menuViewProfile";
            this.menuViewProfile.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.V)));
            this.menuViewProfile.Size = new System.Drawing.Size(201, 22);
            this.menuViewProfile.Text = "&Profile...";
            this.menuViewProfile.Click += new System.EventHandler(this.menuViewProfile_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(198, 6);
            // 
            // menuViewShowPlainText
            // 
            this.menuViewShowPlainText.Name = "menuViewShowPlainText";
            this.menuViewShowPlainText.Size = new System.Drawing.Size(201, 22);
            this.menuViewShowPlainText.Text = "Show Plain &Text";
            this.menuViewShowPlainText.Click += new System.EventHandler(this.menuViewShowPlainText_Click);
            // 
            // menuViewShowInlineImages
            // 
            this.menuViewShowInlineImages.Name = "menuViewShowInlineImages";
            this.menuViewShowInlineImages.Size = new System.Drawing.Size(201, 22);
            this.menuViewShowInlineImages.Text = "Show &Inline Images";
            this.menuViewShowInlineImages.Click += new System.EventHandler(this.menuViewShowInlineImages_Click);
            // 
            // menuViewShowIgnoredMessages
            // 
            this.menuViewShowIgnoredMessages.Name = "menuViewShowIgnoredMessages";
            this.menuViewShowIgnoredMessages.Size = new System.Drawing.Size(201, 22);
            this.menuViewShowIgnoredMessages.Text = "Show I&gnored Messages";
            this.menuViewShowIgnoredMessages.Click += new System.EventHandler(this.menuViewShowIgnoredMessages_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(198, 6);
            // 
            // menuViewPreviousMessage
            // 
            this.menuViewPreviousMessage.Name = "menuViewPreviousMessage";
            this.menuViewPreviousMessage.Size = new System.Drawing.Size(201, 22);
            this.menuViewPreviousMessage.Text = "Bac&ktrack";
            this.menuViewPreviousMessage.Click += new System.EventHandler(this.menuViewPreviousMessage_Click);
            // 
            // menuViewNextUnread
            // 
            this.menuViewNextUnread.Name = "menuViewNextUnread";
            this.menuViewNextUnread.Size = new System.Drawing.Size(201, 22);
            this.menuViewNextUnread.Text = "&Next Unread";
            this.menuViewNextUnread.Click += new System.EventHandler(this.menuViewNextUnread_Click);
            // 
            // menuViewNextUnreadPriority
            // 
            this.menuViewNextUnreadPriority.Name = "menuViewNextUnreadPriority";
            this.menuViewNextUnreadPriority.Size = new System.Drawing.Size(201, 22);
            this.menuViewNextUnreadPriority.Text = "Next Unread Priorit&y";
            this.menuViewNextUnreadPriority.Click += new System.EventHandler(this.menuViewNextUnreadPriority_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(198, 6);
            // 
            // menuViewPreviousRoot
            // 
            this.menuViewPreviousRoot.Name = "menuViewPreviousRoot";
            this.menuViewPreviousRoot.Size = new System.Drawing.Size(201, 22);
            this.menuViewPreviousRoot.Text = "Previous R&oot";
            this.menuViewPreviousRoot.Click += new System.EventHandler(this.menuViewPreviousRoot_Click);
            // 
            // menuViewNextRoot
            // 
            this.menuViewNextRoot.Name = "menuViewNextRoot";
            this.menuViewNextRoot.Size = new System.Drawing.Size(201, 22);
            this.menuViewNextRoot.Text = "Ne&xt Root";
            this.menuViewNextRoot.Click += new System.EventHandler(this.menuViewNextRoot_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(198, 6);
            // 
            // menuViewAllTopics
            // 
            this.menuViewAllTopics.Name = "menuViewAllTopics";
            this.menuViewAllTopics.Size = new System.Drawing.Size(201, 22);
            this.menuViewAllTopics.Text = "&All Topics";
            this.menuViewAllTopics.Click += new System.EventHandler(this.menuViewAllTopics_Click);
            // 
            // menuViewRecentTopics
            // 
            this.menuViewRecentTopics.Name = "menuViewRecentTopics";
            this.menuViewRecentTopics.Size = new System.Drawing.Size(201, 22);
            this.menuViewRecentTopics.Text = "Re&cent Topics";
            this.menuViewRecentTopics.Click += new System.EventHandler(this.menuViewRecentTopics_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(198, 6);
            // 
            // menuViewShowToolBar
            // 
            this.menuViewShowToolBar.Name = "menuViewShowToolBar";
            this.menuViewShowToolBar.Size = new System.Drawing.Size(201, 22);
            this.menuViewShowToolBar.Text = "S&how Toolbar";
            this.menuViewShowToolBar.Click += new System.EventHandler(this.menuViewShowToolBar_Click);
            // 
            // menuViewCustomiseToolbar
            // 
            this.menuViewCustomiseToolbar.Name = "menuViewCustomiseToolbar";
            this.menuViewCustomiseToolbar.Size = new System.Drawing.Size(201, 22);
            this.menuViewCustomiseToolbar.Text = "C&ustomise Toolbar...";
            this.menuViewCustomiseToolbar.Click += new System.EventHandler(this.menuViewCustomiseToolbar_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(198, 6);
            // 
            // menuViewShowStatusBar
            // 
            this.menuViewShowStatusBar.Name = "menuViewShowStatusBar";
            this.menuViewShowStatusBar.Size = new System.Drawing.Size(201, 22);
            this.menuViewShowStatusBar.Text = "Show Status &Bar";
            this.menuViewShowStatusBar.Click += new System.EventHandler(this.menuViewShowStatusBar_Click);
            // 
            // menuMessage
            // 
            this.menuMessage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMessageNewMessage,
            this.menuMessageReply,
            this.menuMessageEdit,
            this.toolStripMenuItem12,
            this.markToolStripMenuItem,
            this.menuMessageMarkThreadRead,
            this.toolStripMenuItem14,
            this.menuMessageGoTo,
            this.menuMessageGoToOriginal,
            this.toolStripMenuItem15,
            this.menuMessageWithdraw,
            this.toolStripSeparator13,
            this.menuMessageBlock});
            this.menuMessage.Name = "menuMessage";
            this.menuMessage.Size = new System.Drawing.Size(65, 20);
            this.menuMessage.Text = "&Message";
            // 
            // menuMessageNewMessage
            // 
            this.menuMessageNewMessage.Name = "menuMessageNewMessage";
            this.menuMessageNewMessage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuMessageNewMessage.Size = new System.Drawing.Size(190, 22);
            this.menuMessageNewMessage.Text = "&New Message";
            this.menuMessageNewMessage.Click += new System.EventHandler(this.menuMessageNewMessage_Click);
            // 
            // menuMessageReply
            // 
            this.menuMessageReply.Name = "menuMessageReply";
            this.menuMessageReply.Size = new System.Drawing.Size(190, 22);
            this.menuMessageReply.Text = "&Reply...";
            this.menuMessageReply.Click += new System.EventHandler(this.menuMessageReply_Click);
            // 
            // menuMessageEdit
            // 
            this.menuMessageEdit.Name = "menuMessageEdit";
            this.menuMessageEdit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.menuMessageEdit.Size = new System.Drawing.Size(190, 22);
            this.menuMessageEdit.Text = "&Edit...";
            this.menuMessageEdit.Click += new System.EventHandler(this.menuMessageEdit_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(187, 6);
            // 
            // markToolStripMenuItem
            // 
            this.markToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMessageMarkUnread,
            this.menuMessageMarkPriority,
            this.menuMessageMarkIgnored,
            this.toolStripMenuItem13,
            this.menuMessageMarkReadLock,
            this.menuMessageMarkStarred});
            this.markToolStripMenuItem.Name = "markToolStripMenuItem";
            this.markToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.markToolStripMenuItem.Text = "&Mark";
            // 
            // menuMessageMarkUnread
            // 
            this.menuMessageMarkUnread.Name = "menuMessageMarkUnread";
            this.menuMessageMarkUnread.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
            this.menuMessageMarkUnread.Size = new System.Drawing.Size(225, 22);
            this.menuMessageMarkUnread.Text = "As &NextUnread";
            this.menuMessageMarkUnread.Click += new System.EventHandler(this.menuMessageMarkUnread_Click);
            // 
            // menuMessageMarkPriority
            // 
            this.menuMessageMarkPriority.Name = "menuMessageMarkPriority";
            this.menuMessageMarkPriority.Size = new System.Drawing.Size(225, 22);
            this.menuMessageMarkPriority.Text = "As &Priority";
            this.menuMessageMarkPriority.Click += new System.EventHandler(this.menuMessageMarkPriority_Click);
            // 
            // menuMessageMarkIgnored
            // 
            this.menuMessageMarkIgnored.Name = "menuMessageMarkIgnored";
            this.menuMessageMarkIgnored.Size = new System.Drawing.Size(225, 22);
            this.menuMessageMarkIgnored.Text = "As &Ignored";
            this.menuMessageMarkIgnored.Click += new System.EventHandler(this.menuMessageMarkIgnored_Click);
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(222, 6);
            // 
            // menuMessageMarkReadLock
            // 
            this.menuMessageMarkReadLock.Image = global::CIXReader.Properties.Resources.ReadLock;
            this.menuMessageMarkReadLock.Name = "menuMessageMarkReadLock";
            this.menuMessageMarkReadLock.Size = new System.Drawing.Size(225, 22);
            this.menuMessageMarkReadLock.Text = "Set Read &Lock";
            this.menuMessageMarkReadLock.Click += new System.EventHandler(this.menuMessageMarkReadLock_Click);
            // 
            // menuMessageMarkStarred
            // 
            this.menuMessageMarkStarred.Image = global::CIXReader.Properties.Resources.ActiveStar;
            this.menuMessageMarkStarred.Name = "menuMessageMarkStarred";
            this.menuMessageMarkStarred.Size = new System.Drawing.Size(225, 22);
            this.menuMessageMarkStarred.Text = "&Flag";
            this.menuMessageMarkStarred.Click += new System.EventHandler(this.menuMessageMarkStarred_Click);
            // 
            // menuMessageMarkThreadRead
            // 
            this.menuMessageMarkThreadRead.Name = "menuMessageMarkThreadRead";
            this.menuMessageMarkThreadRead.Size = new System.Drawing.Size(190, 22);
            this.menuMessageMarkThreadRead.Text = "Mark &Thread Read";
            this.menuMessageMarkThreadRead.Click += new System.EventHandler(this.menuMessageMarkThreadRead_Click);
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size(187, 6);
            // 
            // menuMessageGoTo
            // 
            this.menuMessageGoTo.Name = "menuMessageGoTo";
            this.menuMessageGoTo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.menuMessageGoTo.Size = new System.Drawing.Size(190, 22);
            this.menuMessageGoTo.Text = "&Go To...";
            this.menuMessageGoTo.Click += new System.EventHandler(this.menuMessageGoTo_Click);
            // 
            // menuMessageGoToOriginal
            // 
            this.menuMessageGoToOriginal.Name = "menuMessageGoToOriginal";
            this.menuMessageGoToOriginal.Size = new System.Drawing.Size(190, 22);
            this.menuMessageGoToOriginal.Text = "Go To &Original";
            this.menuMessageGoToOriginal.Click += new System.EventHandler(this.menuMessageGoToOriginal_Click);
            // 
            // toolStripMenuItem15
            // 
            this.toolStripMenuItem15.Name = "toolStripMenuItem15";
            this.toolStripMenuItem15.Size = new System.Drawing.Size(187, 6);
            // 
            // menuMessageWithdraw
            // 
            this.menuMessageWithdraw.Name = "menuMessageWithdraw";
            this.menuMessageWithdraw.Size = new System.Drawing.Size(190, 22);
            this.menuMessageWithdraw.Text = "&Withdraw";
            this.menuMessageWithdraw.Click += new System.EventHandler(this.menuMessageWithdraw_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(187, 6);
            // 
            // menuMessageBlock
            // 
            this.menuMessageBlock.Name = "menuMessageBlock";
            this.menuMessageBlock.Size = new System.Drawing.Size(190, 22);
            this.menuMessageBlock.Text = "Block";
            this.menuMessageBlock.Click += new System.EventHandler(this.menuMessageBlock_Click);
            // 
            // menuFolder
            // 
            this.menuFolder.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFolderJoinForum,
            this.toolStripMenuItem16,
            this.menuFolderDelete,
            this.menuFolderResign,
            this.menuFolderManage,
            this.menuFolderParticipants,
            this.menuFolderMarkAllRead,
            this.toolStripMenuItem17,
            this.menuFolderRefresh});
            this.menuFolder.Name = "menuFolder";
            this.menuFolder.Size = new System.Drawing.Size(52, 20);
            this.menuFolder.Text = "Fo&lder";
            // 
            // menuFolderJoinForum
            // 
            this.menuFolderJoinForum.Name = "menuFolderJoinForum";
            this.menuFolderJoinForum.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
            this.menuFolderJoinForum.Size = new System.Drawing.Size(188, 22);
            this.menuFolderJoinForum.Text = "&Join Forum...";
            this.menuFolderJoinForum.Click += new System.EventHandler(this.menuFolderJoinForum_Click);
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new System.Drawing.Size(185, 6);
            // 
            // menuFolderDelete
            // 
            this.menuFolderDelete.Name = "menuFolderDelete";
            this.menuFolderDelete.Size = new System.Drawing.Size(188, 22);
            this.menuFolderDelete.Text = "&Delete...";
            this.menuFolderDelete.Click += new System.EventHandler(this.menuFolderDelete_Click);
            // 
            // menuFolderResign
            // 
            this.menuFolderResign.Name = "menuFolderResign";
            this.menuFolderResign.Size = new System.Drawing.Size(188, 22);
            this.menuFolderResign.Text = "&Resign...";
            this.menuFolderResign.Click += new System.EventHandler(this.menuFolderResign_Click);
            // 
            // menuFolderManage
            // 
            this.menuFolderManage.Name = "menuFolderManage";
            this.menuFolderManage.Size = new System.Drawing.Size(188, 22);
            this.menuFolderManage.Text = "&Manage...";
            this.menuFolderManage.Click += new System.EventHandler(this.menuFolderManage_Click);
            // 
            // menuFolderParticipants
            // 
            this.menuFolderParticipants.Name = "menuFolderParticipants";
            this.menuFolderParticipants.Size = new System.Drawing.Size(188, 22);
            this.menuFolderParticipants.Text = "&Participants";
            this.menuFolderParticipants.Click += new System.EventHandler(this.menuFolderParticipants_Click);
            // 
            // menuFolderMarkAllRead
            // 
            this.menuFolderMarkAllRead.Name = "menuFolderMarkAllRead";
            this.menuFolderMarkAllRead.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.menuFolderMarkAllRead.Size = new System.Drawing.Size(188, 22);
            this.menuFolderMarkAllRead.Text = "Mar&k All Read";
            this.menuFolderMarkAllRead.Click += new System.EventHandler(this.menuFolderMarkAllRead_Click);
            // 
            // toolStripMenuItem17
            // 
            this.toolStripMenuItem17.Name = "toolStripMenuItem17";
            this.toolStripMenuItem17.Size = new System.Drawing.Size(185, 6);
            // 
            // menuFolderRefresh
            // 
            this.menuFolderRefresh.Name = "menuFolderRefresh";
            this.menuFolderRefresh.Size = new System.Drawing.Size(188, 22);
            this.menuFolderRefresh.Text = "Refres&h";
            this.menuFolderRefresh.Click += new System.EventHandler(this.menuFolderRefresh_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpCheckForUpdates,
            this.toolStripSeparator8,
            this.menuHelpDiagnostics,
            this.menuHelpCIXSupport,
            this.menuHelpViewChangeLog,
            this.menuHelpKeyboardHelp,
            this.toolStripSeparator9,
            this.menuCIXReaderAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(44, 20);
            this.menuHelp.Text = "&Help";
            // 
            // menuHelpCheckForUpdates
            // 
            this.menuHelpCheckForUpdates.Name = "menuHelpCheckForUpdates";
            this.menuHelpCheckForUpdates.Size = new System.Drawing.Size(180, 22);
            this.menuHelpCheckForUpdates.Text = "&Check For Updates";
            this.menuHelpCheckForUpdates.Click += new System.EventHandler(this.mainCheckForUpdates_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(177, 6);
            // 
            // menuHelpDiagnostics
            // 
            this.menuHelpDiagnostics.Name = "menuHelpDiagnostics";
            this.menuHelpDiagnostics.Size = new System.Drawing.Size(180, 22);
            this.menuHelpDiagnostics.Text = "&Diagnostics";
            this.menuHelpDiagnostics.Click += new System.EventHandler(this.mainDiagnostics_Click);
            // 
            // menuHelpCIXSupport
            // 
            this.menuHelpCIXSupport.Name = "menuHelpCIXSupport";
            this.menuHelpCIXSupport.Size = new System.Drawing.Size(180, 22);
            this.menuHelpCIXSupport.Text = "CIX &Support";
            this.menuHelpCIXSupport.Click += new System.EventHandler(this.mainSupport_Click);
            // 
            // menuHelpViewChangeLog
            // 
            this.menuHelpViewChangeLog.Name = "menuHelpViewChangeLog";
            this.menuHelpViewChangeLog.Size = new System.Drawing.Size(180, 22);
            this.menuHelpViewChangeLog.Text = "&View Change Log";
            this.menuHelpViewChangeLog.Click += new System.EventHandler(this.menuHelpViewChangeLog_Click);
            // 
            // menuHelpKeyboardHelp
            // 
            this.menuHelpKeyboardHelp.Name = "menuHelpKeyboardHelp";
            this.menuHelpKeyboardHelp.Size = new System.Drawing.Size(180, 22);
            this.menuHelpKeyboardHelp.Text = "&Keyboard Help";
            this.menuHelpKeyboardHelp.Click += new System.EventHandler(this.mainKeyboardHelp_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(177, 6);
            // 
            // menuCIXReaderAbout
            // 
            this.menuCIXReaderAbout.Name = "menuCIXReaderAbout";
            this.menuCIXReaderAbout.Size = new System.Drawing.Size(180, 22);
            this.menuCIXReaderAbout.Text = "&About CIXReader";
            this.menuCIXReaderAbout.Click += new System.EventHandler(this.mainAbout_Click);
            // 
            // menuViewSortByDate
            // 
            this.menuViewSortByDate.Name = "menuViewSortByDate";
            this.menuViewSortByDate.Size = new System.Drawing.Size(136, 22);
            this.menuViewSortByDate.Text = "&Date";
            this.menuViewSortByDate.Click += new System.EventHandler(this.viewSortByDate_Click);
            // 
            // menuViewSortByAuthor
            // 
            this.menuViewSortByAuthor.Name = "menuViewSortByAuthor";
            this.menuViewSortByAuthor.Size = new System.Drawing.Size(136, 22);
            this.menuViewSortByAuthor.Text = "&Author";
            this.menuViewSortByAuthor.Click += new System.EventHandler(this.viewSortByAuthor_Click);
            // 
            // menuViewSortBySubject
            // 
            this.menuViewSortBySubject.Name = "menuViewSortBySubject";
            this.menuViewSortBySubject.Size = new System.Drawing.Size(136, 22);
            this.menuViewSortBySubject.Text = "&Subject";
            this.menuViewSortBySubject.Click += new System.EventHandler(this.viewSortBySubject_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(133, 6);
            // 
            // menuViewSortAscendingForTopic
            // 
            this.menuViewSortAscendingForTopic.Name = "menuViewSortAscendingForTopic";
            this.menuViewSortAscendingForTopic.Size = new System.Drawing.Size(136, 22);
            this.menuViewSortAscendingForTopic.Text = "Asc&ending";
            this.menuViewSortAscendingForTopic.Click += new System.EventHandler(this.viewSortAscending_Click);
            // 
            // menuViewSortDescendingForTopic
            // 
            this.menuViewSortDescendingForTopic.Name = "menuViewSortDescendingForTopic";
            this.menuViewSortDescendingForTopic.Size = new System.Drawing.Size(136, 22);
            this.menuViewSortDescendingForTopic.Text = "Desce&nding";
            this.menuViewSortDescendingForTopic.Click += new System.EventHandler(this.viewSortDescending_Click);
            // 
            // menuViewSortByTopic
            // 
            this.menuViewSortByTopic.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewSortByDate,
            this.menuViewSortByAuthor,
            this.menuViewSortBySubject,
            this.toolStripSeparator7,
            this.menuViewSortAscendingForTopic,
            this.menuViewSortDescendingForTopic});
            this.menuViewSortByTopic.Name = "menuViewSortByTopic";
            this.menuViewSortByTopic.Size = new System.Drawing.Size(137, 120);
            // 
            // menuViewSortByDirectory
            // 
            this.menuViewSortByDirectory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewSortByName,
            this.menuViewSortByPopularity,
            this.menuViewSortByTitle,
            this.menuViewSortBySubCategory,
            this.toolStripSeparator6,
            this.menuViewSortAscendingForDirectory,
            this.menuViewSortDescendingForDirectory});
            this.menuViewSortByDirectory.Name = "menuViewSortByDirectory";
            this.menuViewSortByDirectory.Size = new System.Drawing.Size(143, 142);
            // 
            // menuViewSortByName
            // 
            this.menuViewSortByName.Name = "menuViewSortByName";
            this.menuViewSortByName.Size = new System.Drawing.Size(142, 22);
            this.menuViewSortByName.Text = "&Name";
            this.menuViewSortByName.Click += new System.EventHandler(this.menuViewSortByName_Click);
            // 
            // menuViewSortByPopularity
            // 
            this.menuViewSortByPopularity.Name = "menuViewSortByPopularity";
            this.menuViewSortByPopularity.Size = new System.Drawing.Size(142, 22);
            this.menuViewSortByPopularity.Text = "&Popularity";
            this.menuViewSortByPopularity.Click += new System.EventHandler(this.menuViewSortByPopularity_Click);
            // 
            // menuViewSortByTitle
            // 
            this.menuViewSortByTitle.Name = "menuViewSortByTitle";
            this.menuViewSortByTitle.Size = new System.Drawing.Size(142, 22);
            this.menuViewSortByTitle.Text = "&Description";
            this.menuViewSortByTitle.Click += new System.EventHandler(this.menuViewSortByTitle_Click);
            // 
            // menuViewSortBySubCategory
            // 
            this.menuViewSortBySubCategory.Name = "menuViewSortBySubCategory";
            this.menuViewSortBySubCategory.Size = new System.Drawing.Size(142, 22);
            this.menuViewSortBySubCategory.Text = "SubCategory";
            this.menuViewSortBySubCategory.Click += new System.EventHandler(this.menuViewSortBySubCategory_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(139, 6);
            // 
            // menuViewSortAscendingForDirectory
            // 
            this.menuViewSortAscendingForDirectory.Name = "menuViewSortAscendingForDirectory";
            this.menuViewSortAscendingForDirectory.Size = new System.Drawing.Size(142, 22);
            this.menuViewSortAscendingForDirectory.Text = "Asc&ending";
            this.menuViewSortAscendingForDirectory.Click += new System.EventHandler(this.viewSortAscending_Click);
            // 
            // menuViewSortDescendingForDirectory
            // 
            this.menuViewSortDescendingForDirectory.Name = "menuViewSortDescendingForDirectory";
            this.menuViewSortDescendingForDirectory.Size = new System.Drawing.Size(142, 22);
            this.menuViewSortDescendingForDirectory.Text = "Desce&nding";
            this.menuViewSortDescendingForDirectory.Click += new System.EventHandler(this.viewSortDescending_Click);
            // 
            // mainStatusBar
            // 
            this.mainStatusBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainStatusBar.BackColor = System.Drawing.Color.Gainsboro;
            this.mainStatusBar.BottomBorderWidth = 0;
            this.mainStatusBar.Controls.Add(this.mainStatusText);
            this.mainStatusBar.Controls.Add(this.mainProgress);
            this.mainStatusBar.Gradient = false;
            this.mainStatusBar.LeftBorderWidth = 0;
            this.mainStatusBar.Location = new System.Drawing.Point(0, 584);
            this.mainStatusBar.Name = "mainStatusBar";
            this.mainStatusBar.RightBorderWidth = 0;
            this.mainStatusBar.Size = new System.Drawing.Size(895, 22);
            this.mainStatusBar.TabIndex = 1;
            this.mainStatusBar.TopBorderWidth = 1;
            this.mainStatusBar.Visible = false;
            // 
            // mainStatusText
            // 
            this.mainStatusText.AutoSize = true;
            this.mainStatusText.ForeColor = System.Drawing.SystemColors.WindowText;
            this.mainStatusText.Location = new System.Drawing.Point(29, 5);
            this.mainStatusText.Name = "mainStatusText";
            this.mainStatusText.Size = new System.Drawing.Size(0, 13);
            this.mainStatusText.TabIndex = 1;
            this.mainStatusText.Visible = false;
            // 
            // mainProgress
            // 
            this.mainProgress.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.mainProgress.Location = new System.Drawing.Point(3, 1);
            this.mainProgress.Name = "mainProgress";
            this.mainProgress.Percentage = 0F;
            this.mainProgress.Size = new System.Drawing.Size(19, 19);
            this.mainProgress.TabIndex = 0;
            this.mainProgress.Text = "crProgress1";
            this.mainProgress.Visible = false;
            // 
            // refreshtCtrlTToolStripMenuItem
            // 
            this.refreshtCtrlTToolStripMenuItem.Name = "refreshtCtrlTToolStripMenuItem";
            this.refreshtCtrlTToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(895, 605);
            this.Controls.Add(this.mainMenubar);
            this.Controls.Add(this.mainStatusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenubar;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "(Title)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.mainPopupMenu.ResumeLayout(false);
            this.mainMenubar.ResumeLayout(false);
            this.mainMenubar.PerformLayout();
            this.menuViewSortByTopic.ResumeLayout(false);
            this.menuViewSortByDirectory.ResumeLayout(false);
            this.mainStatusBar.ResumeLayout(false);
            this.mainStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ContextMenuStrip mainPopupMenu;
        private ToolStripMenuItem mainAbout;
        private ToolStripMenuItem mainLogoff;
        private ToolStripMenuItem mainExit;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem mainOffline;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem mainSupport;
        private ToolStripMenuItem mainKeyboardHelp;
        private ToolStripMenuItem mainDiagnostics;
        private ToolStripMenuItem mainSettings;
        private ToolStripMenuItem mainCheckForUpdates;
        private CRPanel mainStatusBar;
        private CRProgress mainProgress;
        private Label mainStatusText;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem mainInstallScript;
        private MenuStrip mainMenubar;
        private ToolStripMenuItem menuFile;
        private ToolStripMenuItem menuFileNewPrivateMessage;
        private ToolStripMenuItem menuFileReply;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem menuFileOffline;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem menuFilePageSetup;
        private ToolStripMenuItem menuFilePrint;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem menuFileSignOut;
        private ToolStripMenuItem menuFileRefresh;
        private ToolStripMenuItem menuEdit;
        private ToolStripMenuItem menuView;
        private ToolStripMenuItem menuMessage;
        private ToolStripMenuItem menuFolder;
        private ToolStripMenuItem menuHelp;
        private ToolStripMenuItem menuHelpCheckForUpdates;
        private ToolStripMenuItem menuHelpCIXSupport;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripMenuItem menuFileExit;
        private ToolStripMenuItem menuHelpDiagnostics;
        private ToolStripMenuItem menuHelpKeyboardHelp;
        private ToolStripMenuItem menuViewProfile;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripMenuItem menuViewShowPlainText;
        private ToolStripMenuItem menuViewShowInlineImages;
        private ToolStripSeparator toolStripMenuItem9;
        private ToolStripMenuItem menuViewPreviousMessage;
        private ToolStripMenuItem menuViewNextUnread;
        private ToolStripMenuItem menuViewNextUnreadPriority;
        private ToolStripSeparator toolStripMenuItem10;
        private ToolStripMenuItem menuViewPreviousRoot;
        private ToolStripMenuItem menuViewNextRoot;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripMenuItem menuViewAllTopics;
        private ToolStripMenuItem menuViewRecentTopics;
        private ToolStripSeparator toolStripMenuItem11;
        private ToolStripMenuItem menuViewShowStatusBar;
        private ToolStripMenuItem menuMessageNewMessage;
        private ToolStripMenuItem menuMessageReply;
        private ToolStripMenuItem menuMessageEdit;
        private ToolStripSeparator toolStripMenuItem12;
        private ToolStripMenuItem markToolStripMenuItem;
        private ToolStripMenuItem menuMessageMarkUnread;
        private ToolStripMenuItem menuMessageMarkPriority;
        private ToolStripMenuItem menuMessageMarkIgnored;
        private ToolStripSeparator toolStripMenuItem13;
        private ToolStripMenuItem menuMessageMarkReadLock;
        private ToolStripMenuItem menuMessageMarkStarred;
        private ToolStripMenuItem menuMessageMarkThreadRead;
        private ToolStripSeparator toolStripMenuItem14;
        private ToolStripMenuItem menuMessageGoTo;
        private ToolStripMenuItem menuMessageGoToOriginal;
        private ToolStripSeparator toolStripMenuItem15;
        private ToolStripMenuItem menuMessageWithdraw;
        private ToolStripMenuItem menuFolderJoinForum;
        private ToolStripSeparator toolStripMenuItem16;
        private ToolStripMenuItem menuFolderDelete;
        private ToolStripMenuItem menuFolderResign;
        private ToolStripMenuItem menuFolderMarkAllRead;
        private ToolStripSeparator toolStripMenuItem17;
        private ToolStripMenuItem menuFolderRefresh;
        private ToolStripMenuItem menuEditCopy;
        private ToolStripMenuItem menuEditCopyLink;
        private ToolStripMenuItem menuEditSelectAll;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem menuEditQuote;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem menuCIXReaderSettings;
        private ToolStripMenuItem menuCIXReaderAccount;
        private ToolStripMenuItem menuCIXReaderAbout;
        private ToolStripMenuItem menuFolderParticipants;
        private ToolStripMenuItem menuViewSortBy;
        private ToolStripMenuItem menuViewSortByDate;
        private ToolStripMenuItem menuViewSortByAuthor;
        private ToolStripMenuItem menuViewSortBySubject;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem menuViewSortAscendingForTopic;
        private ToolStripMenuItem menuViewSortDescendingForTopic;
        private ContextMenuStrip menuViewSortByTopic;
        private ContextMenuStrip menuViewSortByDirectory;
        private ToolStripMenuItem menuViewSortByName;
        private ToolStripMenuItem menuViewSortByTitle;
        private ToolStripMenuItem menuViewSortByPopularity;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem menuViewSortAscendingForDirectory;
        private ToolStripMenuItem menuViewSortDescendingForDirectory;
        private ToolStripMenuItem menuViewSortBySubCategory;
        private ToolStripMenuItem menuViewShowToolBar;
        private ToolStripMenuItem menuFolderManage;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem menuHelpViewChangeLog;
        private ToolStripSeparator toolStripSeparator9;
        private ToolStripMenuItem mainViewChangeLog;
        private ToolStripMenuItem menuFileReplyByMail;
        private ToolStripMenuItem menuViewShowIgnoredMessages;
        private ToolStripSeparator toolStripSeparator10;
        private ToolStripSeparator toolStripSeparator11;
        private ToolStripSeparator toolStripSeparator12;
        private ToolStripMenuItem menuViewGroupByConv;
        private ToolStripMenuItem menuViewCollapseConv;
        private ToolStripMenuItem menuViewCustomiseToolbar;
        private ToolStripSeparator toolStripSeparator13;
        private ToolStripMenuItem menuMessageBlock;
        private ToolStripMenuItem refreshtCtrlTToolStripMenuItem;
    }
}