using System;
using System.Windows.Forms;

namespace AppLimit.NetSparkle
{
    public partial class NetSparkleMainWindows : Form
    {
        public NetSparkleMainWindows()
        {
            InitializeComponent();
        }

        public void Report(String message)
        {
            DateTime c = DateTime.Now;

            lstActions.Items.Add("[" + c.ToLongTimeString() +"." + c.Millisecond + "] " + message);
        }
    }
}
