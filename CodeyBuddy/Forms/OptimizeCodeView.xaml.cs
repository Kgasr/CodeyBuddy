using EnvDTE;
using EnvDTE80;
using System.Windows;
using System.Windows.Controls;

namespace CodeyBuddy
{
    /// <summary>
    /// Interaction logic for OptimizeCodeView.xaml
    /// </summary>
    public partial class OptimizeCodeView : System.Windows.Window, IDisposable
    {
        public static string stage = "";

        private bool disposed = false;
        public static string UserCode { get; set; }
        public static string OptimizedCode { get; set; }

        public OptimizeCodeView(string userCode, string optimizedCode)
        {
            InitializeComponent();
            Closed += OptimizeCodeView_Closed;
            Closing += OptimizeCodeView_Closed;
            UserCode = userCode;
            OptimizedCode = optimizedCode;
            HideLoadingPanel();
        }


        private void OptimizeCodeView_Closed(object sender, EventArgs e)
        {
            Dispose();
        }

        private void OptimizeCodeView_Loaded(object sender, RoutedEventArgs e)
        {
            var usrTxtBox = (TextBox)FindName("userCodeTextBox");
            usrTxtBox.Text = UserCode;
            usrTxtBox.IsEnabled = true;
            usrTxtBox.IsReadOnly = false;
            var optimizedTxtBox = (TextBox)FindName("optimizedCodeTextBox");
            optimizedTxtBox.Text = OptimizedCode;
            HideLoadingPanel();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            ShowLoadingPanel();
            var optimizedTxtBox = (TextBox)FindName("optimizedCodeTextBox");
            OptimizedCode = optimizedTxtBox.Text;
            stage = "Apply";
            InvokeCommand("CodeyBuddy.OptimizeCode", true);
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

        private async void btnReevaluate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Show the loading panel
                ShowLoadingPanel();

                var usrTxtBox = (TextBox)FindName("userCodeTextBox");
                UserCode = usrTxtBox.Text;

                var optimizedTxtBox = (TextBox)FindName("optimizedCodeTextBox");
                optimizedTxtBox.Text = "";
                stage = "Reeval";

                // Execute the command asynchronously
                await Task.Run(async () =>
                {
                    await ReevalAsync();
                });
            }
            catch (Exception)
            {
                throw;
            }
         
        }

        private async Task ReevalAsync()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Get the current instance of Visual Studio
            DTE2 dte = await AsyncServiceProvider.GlobalProvider.GetServiceAsync(typeof(DTE)) as DTE2;

            if (dte != null)
            {
                dte.ExecuteCommand("CodeyBuddy.OptimizeCode");
                stage = "";
            }

        }

        private void InvokeCommand(string commandName, Boolean flag)
        {
            EnvDTE80.DTE2 dte = Package.GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
            if (dte != null)
            {
                dte.ExecuteCommand(commandName);
                stage = "";
                if (flag)
                {
                    Close();
                }
            }
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
            var usrTxtBox = (TextBox)FindName("userCodeTextBox");
            usrTxtBox.Text = usrCode;

            var optimizedTxtBox = (TextBox)FindName("optimizedCodeTextBox");
            optimizedTxtBox.Text = improvedCode;
            HideLoadingPanel();
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

        ~OptimizeCodeView()
        {
            Dispose(false);
        }

    }


}

