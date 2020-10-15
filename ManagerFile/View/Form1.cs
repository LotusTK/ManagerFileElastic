using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManagerFile
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            listView.Columns.Add("Name");
            listView.Columns.Add("DirectoryName");
            listView.Columns.Add("Extension");
            listView.Columns.Add("CreationTime");


            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }


        private void AddToListView(string pathFile)
        {
            FileInfo file = new FileInfo(pathFile);
            ListViewItem item = new ListViewItem(file.Name);
            item.SubItems.Add(file.DirectoryName);
            item.SubItems.Add(file.Extension);
            item.SubItems.Add(file.CreationTime.ToString("dd-MM-yyyy"));

            listView.Invoke((Action)(() => {
                listView.BeginUpdate();
                listView.Items.Add(item);
                listView.EndUpdate();
            }));
        }

        private void scanFiles(string rootDirectory)
        {
            var directories = new string[] { };
            try
            {
                foreach (var file in Directory.GetFiles(rootDirectory, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".doc") || s.EndsWith(".txt") || s.EndsWith(".pdf") || s.EndsWith(".docx")))
                {
                    if (backgroundWorker.CancellationPending)
                    {
                        return;
                    }

                    AddToListView(file);
                }
                directories = Directory.GetDirectories(rootDirectory);
            }
            catch (Exception) 
            {
                //Debug.WriteLine(rootDirectory);
            }
            foreach(var dir in directories)
            {
                try
                {
                    lblProcess.Invoke((Action)(() => {
                        lblProcess.Text = dir.ToString();
                    }));
                    scanFiles(dir);
                }
                catch (Exception)
                {
                    //Debug.WriteLine(rootDirectory);
                }
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string rootDirectory = txtPath.Text.ToString();
            string[] dirs = Directory.GetDirectories(rootDirectory);
            foreach (var file in Directory.GetFiles(rootDirectory, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".doc") || s.EndsWith(".txt") || s.EndsWith(".pdf") || s.EndsWith(".docx")))
            {
                AddToListView(file);
            }
            float lenght = dirs.Length;
            progressBar.Invoke((Action)(() => 
                progressBar.Maximum = dirs.Length
            ));

            for(int i = 0; i < dirs.Length; i++)
            {
                backgroundWorker.ReportProgress((int)(i / lenght * 100));
                scanFiles(dirs[i]);
            }

            backgroundWorker.ReportProgress(100);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!backgroundWorker.CancellationPending)
            {
                lblPercent.Text = e.ProgressPercentage + "%";
                progressBar.PerformStep();
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblProcess.Text = String.Format("{0} files found", listView.Items.Count);
            if(progressBar.Value < progressBar.Maximum)
            {
                lblProcess.Text = "Searching Cancelled." + lblProcess.Text;
            }
            btnStartCancel.Text = "Start";
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnStartCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
            else
            {
                progressBar.Value = progressBar.Minimum;
                btnStartCancel.Text = "Cancel";
                listView.Items.Clear();
                backgroundWorker.RunWorkerAsync();
            }
        }
    }
}
