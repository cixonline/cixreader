using System.Windows.Forms;
using CIXClient;
using CIXClient.Collections;
using CIXReader.Utilities;

namespace CIXReader.Forms {

    public sealed partial class SyncProgress : Form
    {
        public SyncProgress()
        {
            InitializeComponent();
        }

        private void SyncProgress_Shown(object sender, System.EventArgs e)
        {
            CIX.RefreshStatusEnded += SyncProgress_Completed;
            CIX.RunAllTasks();
        }

        /// <summary>
        /// Called when RunAllTasks completes.
        /// </summary>
        private void SyncProgress_Completed(object sender, StatusEventArgs e)
        {
            Platform.UIThread(this, delegate
                {
                    CIX.RefreshStatusEnded -= SyncProgress_Completed;
                    DialogResult = DialogResult.OK;
                    Close();
                });
        }
    }
}
