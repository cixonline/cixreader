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

        private void SyncProgress_Completed(object sender, StatusEventArgs e)
        {
            Platform.UIThread(this, delegate
                {
                    DialogResult = DialogResult.OK;
                    Close();
                });
        }
    }
}
