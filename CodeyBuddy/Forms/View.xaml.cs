using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Text.Editor;

namespace CodeyBuddy
{
    /// <summary>
    /// Interaction logic for View.xaml
    /// </summary>
    public partial class View : System.Windows.Window, IDisposable
    {
        private bool disposed = false;

        public static Boolean callFromView = false;
        public static string UserInput { get; set; }
        public static string Response { get; set; }
        public View(string userInput, string resposne)
        {
            InitializeComponent();
            Closed += View_Closed;
            Closing += View_Closed;
            UserInput = userInput;
            Response = resposne;
            HideLoadingPanel();
            callFromView = false;

        }

        private void test(object sender, KeyEventArgs e)
        {
            var a = e.Key;
        }
        private void View_Closed(object sender, EventArgs e)
        {
            Dispose();
        }

        private async void View_Loaded(object sender, RoutedEventArgs e)
        {
            var usrTxtBox = (TextBox)FindName("userTextBox");
            usrTxtBox.Text = UserInput;
            usrTxtBox.IsEnabled = true;
            usrTxtBox.IsReadOnly = false;
            usrTxtBox.Focusable = true;
            usrTxtBox.AcceptsReturn = true;
            usrTxtBox.AcceptsTab = true;
            usrTxtBox.Focus();

            var resposneTxtBox = (TextBox)FindName("responseTextBox");
            resposneTxtBox.Text = Response;
            HideLoadingPanel();
            callFromView = false;

            // Get the DTE object
            DTE dte = Package.GetGlobalService(typeof(DTE)) as DTE;

            // Get the active document
            Document doc = dte.ActiveDocument;

            // Set the ReadOnly property to true
            doc.ReadOnly = true;

        }

        private async void btnAsk_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // Show the loading panel
                ShowLoadingPanel();

                var usrTxtBox = (TextBox)FindName("userTextBox");
                UserInput = usrTxtBox.Text;

                var resposneTxtBox = (TextBox)FindName("responseTextBox");
                resposneTxtBox.Text = "";
                callFromView = true;
                // Execute the command asynchronously
                await Task.Run(async () =>
                {
                    await AskAsync();
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task AskAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Get the current instance of Visual Studio
            DTE2 dte = await AsyncServiceProvider.GlobalProvider.GetServiceAsync(typeof(DTE)) as DTE2;

            if (dte != null)
            {
                dte.ExecuteCommand("CodeyBuddy.ExplainCode");
                callFromView = false;
            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            callFromView = false;
            userTextBox.Text = "";
            responseTextBox.Text = "";
            UserInput = "";
            Response = "";
            Close();
            Dispose();

            // Get the DTE object
            DTE dte = Package.GetGlobalService(typeof(DTE)) as DTE;

            // Get the active document
            Document doc = dte.ActiveDocument;

            // Set the ReadOnly property to true
            doc.ReadOnly = false;
        }

        public static bool IsWindowOpen<T>() where T : System.Windows.Window
        {
            foreach (System.Windows.Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(T))
                {

                    return true;
                }
            }
            return false;
        }

        public static bool TryUpdateWindow<T>(Action<T> updateAction) where T : System.Windows.Window
        {
            foreach (System.Windows.Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(T))
                {
                    T typedWindow = (T)window;
                    updateAction(typedWindow);
                    return true;
                }
            }
            return false;
        }

        public void UpdateValues(string usrCode, string improvedCode)
        {
            var usrTxtBox = (TextBox)FindName("userTextBox");
            usrTxtBox.Text = usrCode;

            var resposneTxtBox = (TextBox)FindName("responseTextBox");
            resposneTxtBox.Text = improvedCode;
            HideLoadingPanel();
        }

        public void ShowLoadingPanel()
        {
            var loadingPanel = (StackPanel)FindName("LoadingPanel");
            loadingPanel.Visibility = Visibility.Visible;
        }

        private void HideLoadingPanel()
        {
            var loadingPanel = (StackPanel)FindName("LoadingPanel");
            loadingPanel.Visibility = Visibility.Hidden;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Release managed resources here
                }
                // Release unmanaged resources here
                disposed = true;
            }
        }

        ~View()
        {
            Dispose(false);
        }
    }
}
