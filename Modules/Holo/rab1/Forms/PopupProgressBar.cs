using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System;

namespace rab1
{
    class PopupProgressBar
    {
        private static Form progressBarForm;
        private static ProgressBar pBar;

        //Birth
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public PopupProgressBar()
        {
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void show()
        {
            if (progressBarForm == null)
            {
                createForm();
            }

            adjustForm();

            progressBarForm.Show();
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void createForm()
        {
            progressBarForm = new Form();
            progressBarForm.Size = new Size(400, 80);
            progressBarForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            progressBarForm.Closing += progressBarClosed;

            pBar = new ProgressBar();
            progressBarForm.Controls.Add(pBar); 
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void progressBarClosed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            progressBarForm = null;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void adjustForm()
        {
            progressBarForm.StartPosition = FormStartPosition.CenterScreen;
            pBar.Location = new System.Drawing.Point(15, 20);
            pBar.Width = progressBarForm.Size.Width - 30;
            pBar.Height = 30;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void setProgress(int done, int all)
        {
            if (progressBarForm == null)
            {
                return;
            }

            if (done >= all)
            {
                return;
            }

            SetControlPropertyThreadSafe(pBar, "Minimum", 0);
            SetControlPropertyThreadSafe(pBar, "Maximum", all);
            SetControlPropertyThreadSafe(pBar, "Value", done);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void close()
        {
            if (progressBarForm == null)
            {
                return;
            }
            progressBarForm.Close();
            progressBarForm = null;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);

        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
