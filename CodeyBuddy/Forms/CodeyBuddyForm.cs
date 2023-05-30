using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;

namespace CodeyBuddy.Forms
{
    public partial class CodeyBuddyForm : Form
    {
        public static Boolean callFromView = false;
        public static Boolean loading = false;
        public string UserInput
        {
            get { return txtUserInput.Text; }
            set { txtUserInput.Text = value; }
        }
        public string Response
        {
            get { return txtResposne.Text; }
            set { txtResposne.Text = value; }
        }
        public CodeyBuddyForm(string userInput, string resposne)
        {
            InitializeComponent();
            txtUserInput.Text = userInput;
            txtResposne.Text = resposne;
            this.ControlBox = false; 
           // txtResposne.WordWrap = true;
        }

        private void CodeyBuddyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing && loading)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }

       
        private async void btnAsk_Click(object sender, EventArgs e)
        {
            callFromView = true;
            Response = string.Empty;
            ShowLoadingPanel();

            // Execute the command asynchronously
            await Task.Run(async () =>
            {
                await AskAsync();
            });
        }

        private async Task AskAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Get the current instance of Visual Studio
            DTE2 dte = await AsyncServiceProvider.GlobalProvider.GetServiceAsync(typeof(DTE)) as DTE2;

            if (dte != null)
            {
                dte.ExecuteCommand("CodeyBuddy.AskAnything");
                callFromView = false;
            }
        }

        public void ShowLoadingPanel()
        {
            pbBusyIndicator.Visible = true;
            btnAsk.Enabled = false;
            btnCancel.Enabled = false;
            loading = true;
        }

        public void HideLoadingPanel()
        {
            //txtResposne.WordWrap = true;
            pbBusyIndicator.Visible = false;
            btnAsk.Enabled = true;
            btnCancel.Enabled = true;
            loading = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CodeyBuddyForm_Load(object sender, EventArgs e)
        {

        }

        
    }
}
