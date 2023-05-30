using EnvDTE;
using System.Linq;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace CodeyBuddy
{
    [Command(PackageIds.OptimizeCode)]
    internal sealed class OptimizeCode : BaseCommand<OptimizeCode>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            try
            {
                var docView = await VS.Documents.GetActiveDocumentViewAsync();
                //int currentLineNumber = (int)docView?.TextView?.Selection?.ActivePoint.Position.GetContainingLine().LineNumber;
                var selection = docView?.TextView?.Selection?.SelectedSpans?.FirstOrDefault();
                var selectedCode = selection.Value;
                var prompt = selection.GetValueOrDefault().GetText().ToString();
                switch (OptimizeCodeView.stage)
                {
                    case "Apply":
                        await VS.StatusBar.ShowProgressAsync("Processing....!!", 1, 2);
                        await FormatDocumentAsync(docView, selectedCode, OptimizeCodeView.OptimizedCode);
                        await VS.StatusBar.ShowProgressAsync("Processing Completed....!!", 2, 2);
                        break;
                    case "Reeval":
                        await InvokeAPIAsync(OptimizeCodeView.UserCode);
                        break;
                    default:
                        if (!string.IsNullOrEmpty(prompt) && !string.IsNullOrWhiteSpace(prompt))
                        {
                            await VS.StatusBar.ShowProgressAsync("Processing....!!", 1, 2);
                            await InvokeAPIAsync(prompt);
                            await VS.StatusBar.ShowProgressAsync("Processing Completed....!!", 2, 2);
                        }
                        else
                        {
                            throw new Exception("Please select the code for optimization");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                await VS.MessageBox.ShowWarningAsync("CodeyBuddy ERR002 :- " + ex.Message);
                await VS.StatusBar.ClearAsync();
                await VS.StatusBar.ShowProgressAsync("Processing Ended....!!", 2, 2);
            }
        }
        private async Task FormatDocumentAsync(DocumentView docView, object context, string output)
        {
            using (Utilities utilities = new Utilities())
            {
                await utilities.FormatDocumentAsync(docView, context, output, "Optimize");
            }
        }
        private async Task InvokeAPIAsync(string prompt)
        {
            string output;
            if (!string.IsNullOrEmpty(prompt))
            {
                using (Utilities utilities = new Utilities())
                {
                    output = await utilities.InvokeOpenAIAPIAsync(prompt, "Optimize");
                }
                LoadForm(prompt, output);
            }
            else
            {
                throw new Exception("Please supply the prompt/text to execute the Open AI API call");
            }
        }

        private void LoadForm(string prompt, string output)
        {
            if (OptimizeCodeView.TryUpdateWindow<OptimizeCodeView>(w => w.UpdateValues(prompt, output)))
            {
                // The window was already open and its values were updated
            }
            else
            {
                // The window was not open, so you need to open it first
                using (OptimizeCodeView dialog = new OptimizeCodeView(prompt, output))
                {
                    // Open the window if it's not already open
                    dialog.Show();
                    //dialog.ShowLoadingPanel();
                }
            }
        }
    }
}
