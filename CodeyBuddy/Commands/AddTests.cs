using EnvDTE;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Shell;

namespace CodeyBuddy
{
    [Command(PackageIds.AddTests)]
    internal sealed class AddTests : BaseCommand<AddTests>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            try
            {
                var docView = await VS.Documents.GetActiveDocumentViewAsync();
                int currentLinePosition = (int)docView?.TextView?.Selection?.End.Position.GetContainingLine().EndIncludingLineBreak;
                var selection = docView?.TextView?.Selection?.SelectedSpans?.FirstOrDefault();
                string prompt = selection.GetValueOrDefault().GetText().ToString();
                if (!string.IsNullOrEmpty(prompt))
                {
                    using (Utilities utilities = new Utilities())
                    {
                        await VS.StatusBar.ShowProgressAsync("Processing....!!", 1, 4);
                        string output = await utilities.InvokeOpenAIAPIAsync(prompt, "Tests");
                        output = output.Replace(prompt, string.Empty);
                        await VS.StatusBar.ShowProgressAsync("Processing....!!", 2, 4);
                        await utilities.FormatDocumentAsync(docView, currentLinePosition, output, "Tests");
                        await VS.StatusBar.ShowProgressAsync("Processing....!!", 3, 4);
                        await VS.StatusBar.ShowProgressAsync("Processing....!!", 4, 4);
                    }
                }
                else
                {
                    throw new Exception("Please supply the code to add test cases");
                }
            }
            catch (Exception ex)
            {
                await VS.MessageBox.ShowWarningAsync("CodeyBuddy ERR005 :- " + ex.Message);
                await VS.StatusBar.ClearAsync();
                await VS.StatusBar.ShowProgressAsync("Processing Ended....!!", 2, 2);
            }
        }

    }
}
