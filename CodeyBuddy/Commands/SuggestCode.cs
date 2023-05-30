using EnvDTE;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace CodeyBuddy
{
    [Command(PackageIds.SuggestCode)]
    internal sealed class SuggestCode : BaseCommand<SuggestCode>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            try
            {
                var docView = await VS.Documents.GetActiveDocumentViewAsync();
                //int currentLinePosition = (int)docView?.TextView?.Selection?.ActivePoint.Position.GetContainingLine().EndIncludingLineBreak;
                int currentLinePosition = (int)docView?.TextView?.Selection?.End.Position.GetContainingLine().EndIncludingLineBreak;
                var selection = docView?.TextView?.Selection?.SelectedSpans?.FirstOrDefault();
                string prompt = selection.GetValueOrDefault().GetText().ToString();
                if (!string.IsNullOrEmpty(prompt))
                {
                    using (Utilities utilities = new Utilities())
                    {
                        await VS.StatusBar.ShowProgressAsync("Processing....!!", 1, 4);
                        string output = await utilities.InvokeOpenAIAPIAsync(prompt, "Suggest");
                        await VS.StatusBar.ShowProgressAsync("Processing....!!", 2, 4);
                        await utilities.FormatDocumentAsync(docView, currentLinePosition, output, "Suggest");
                        await VS.StatusBar.ShowProgressAsync("Processing....!!", 3, 4);
                        await VS.StatusBar.ShowProgressAsync("Processing....!!", 4, 4);
                    }
                }
                else
                {
                    throw new Exception("Please supply the prompt/text to suggest code");
                }
            }
            catch (Exception ex)
            {
                await VS.MessageBox.ShowWarningAsync("CodeyBuddy ERR001 :- " + ex.Message);
                await VS.StatusBar.ClearAsync();
                await VS.StatusBar.ShowProgressAsync("Processing Ended....!!", 2, 2);
            }
        }

    }
}
