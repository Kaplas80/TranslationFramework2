using System;
using System.ComponentModel;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace TF.GUI.Forms
{
    public partial class WorkingForm : Form
    {
        private readonly BackgroundWorker _worker;
        private readonly bool _autoClose;

        public event DoWorkEventHandler DoWork;

        protected WorkingForm()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;

            _worker = new BackgroundWorker {WorkerReportsProgress = true, WorkerSupportsCancellation = true};

            _worker.RunWorkerCompleted += (sender, args) =>
            {
                if (_autoClose)
                {
                    Close();
                }
                else
                {
                    progressBar1.Visible = false;
                    btnCancel.Visible = false;
                    btnClose.Visible = true;
                }
            };

            _worker.ProgressChanged += (sender, args) => { AddProgress(args.UserState.ToString()); };
        }

        public WorkingForm(ThemeBase theme, string label, bool autoClose = false) : this()
        {
            dockPanel1.Theme = theme;
            Text = label;
            _autoClose = autoClose;
        }

        public void AddProgress(string text)
        {
            textBox1.AppendText(text + Environment.NewLine);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _worker.CancelAsync();
            btnCancel.Enabled = false;
        }

        private void WorkingForm_Shown(object sender, EventArgs e)
        {
            _worker.DoWork += this.DoWork;
            _worker.RunWorkerAsync();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
