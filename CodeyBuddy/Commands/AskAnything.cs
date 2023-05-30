using System.Linq;
using System.Windows.Forms;
using CodeyBuddy.Forms;
using EnvDTE;

namespace CodeyBuddy
{
    [Command(PackageIds.AskAnything)]
    internal sealed class AskAnything : BaseCommand<AskAnything>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            //await VS.MessageBox.ShowWarningAsync("CallOpenAI", "Button clicked");
            try
            {
                var docView = await VS.Documents.GetActiveDocumentViewAsync();
                var selection = docView?.TextView?.Selection?.SelectedSpans?.FirstOrDefault();
                CodeyBuddyForm form = Application.OpenForms.OfType<CodeyBuddyForm>().FirstOrDefault();
                if (form != null && Utilities.CheckFormCall(form, "callFromView"))
                {
                    await InvokeAPIAsync(form.UserInput);
                }
                else
                {
                    if (selection != null)
                    {
                        await VS.StatusBar.ShowProgressAsync("Processing....!!", 1, 2);
                        await InvokeAPIAsync(selection.GetValueOrDefault().GetText().ToString());
                        await VS.StatusBar.ShowProgressAsync("Processing Completed....!!", 2, 2);
                    }
                    else
                    {
                        LoadForm(string.Empty, String.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                await VS.MessageBox.ShowWarningAsync("CodeyBuddy ERR004 :- " + ex.Message);
                await VS.StatusBar.ClearAsync();
                await VS.StatusBar.ShowProgressAsync("Processing Ended....!!", 2, 2);
            }
        }

        private async Task InvokeAPIAsync(string prompt)
        {
            string output = "";
            if (!string.IsNullOrEmpty(prompt))
            {
                using (Utilities utilities = new Utilities())
                {
                    output = await utilities.InvokeOpenAIAPIAsync(prompt, "Advise");
                   // output = output.Replace(".", ".\r\n");
                }
            }
            LoadForm(prompt, output);
        }

        private void LoadForm(string prompt, string output)
        {
            // Check if form is already open
            CodeyBuddyForm form = Application.OpenForms.OfType<CodeyBuddyForm>().FirstOrDefault();
            if (form != null)
            {
                // If the form is already open, update the TextBox
                form.UserInput = prompt;
                form.Response = output;
                form.HideLoadingPanel();

            }
            else
            {
                // If the form is not already open, create a new instance and show it
                form = new CodeyBuddyForm(prompt, output);
                form.Show();
            }
        }
    }
}
