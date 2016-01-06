using OneBuild.Common;
using OneBuild.Config;
using OneBuild.UI.Easyui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneBuild.UI
{
    [ComVisible(true)]
    public partial class Form1 : Form
    {
        EasyUI easyUI = new EasyUI();
        public Form1()
        {
            InitializeComponent();
            Config.Application.Initialize();
            this.MaximizedBounds = Screen.FromControl(this).WorkingArea;
            this.uiContainer.ObjectForScripting = this;
            string mainUI = Path.Combine(Machine.ApplicationDir, "Main.html");
            this.uiContainer.Url = new Uri(mainUI);
        }

        public void Exit()
        {
            this.Close();
        }

        public void Minimize()
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void Maximize()
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void Form1_MaximumSizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.uiContainer.Document.InvokeScript("WindowMaximize");
            }
        }

        public string GetFilesInfo(string path = null)
        {

            DirectoryInfo directoryInfo = null;
            if (!String.IsNullOrEmpty(path))
            {
                directoryInfo = new DirectoryInfo(path);
            }
            FileSystemNodeTree tree = Machine.GetFileSystemTree(directoryInfo);
            string json = easyUI.BuildFileTree(tree, directoryInfo == null);
            File.AppendAllText("1.txt", json);
            return json;
        }

        public string Navigate(string url)
        {
            return easyUI.Navigate(url);
        }

        public void NewProject(string proj)
        {

        }
    }
}
