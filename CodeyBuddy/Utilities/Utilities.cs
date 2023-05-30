using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using Community.VisualStudio.Toolkit;
using CodeyBuddy.Forms;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.VisualStudio.Text;

namespace CodeyBuddy
{
    public class Utilities : IDisposable
    {
        private bool disposed = false;
        private string apiKey, apiUrl, prompt;
        private double temperature = 0.5;
        private int maxTokens = 1000;

        public async Task<string> InvokeOpenAIAPIAsync(string tempPrompt, string command)
        {

            CreatePrompt(tempPrompt, command);
            var requestBody = new
            {
                prompt = prompt,
                max_tokens = maxTokens,
                temperature = temperature
            };
            await GetAPIParametersAsync();
            string output = await OpenAPICallAsync(requestBody);
            //await FormatDocumentAsync(docView, currentLine, output);
            return output;
        }

 
        private void CreatePrompt(string tempPrompt, string command)
        {
            string temp = tempPrompt.Replace("\r\n", "\\n");
            prompt = command switch
            {
                //"Suggest" => tempPrompt.StartsWith("//") ? "Suggest Code for this \\" + (tempPrompt.Substring(2, tempPrompt.Length - 2)).Replace("\r\n", "\\n") : "Suggest Code for this \\" + tempPrompt.Replace("\r\n", "\\n"),
                "Suggest" => "Suggest on below code \\" + temp,
                "Optimize" => "Optimize below code \\" + temp,
                "Explain" => "Explain below code \\" + temp,
                "Tests" => "Create unit tests \\" + temp,
                "Advise" => "Advise on below \\" + temp,
                "" => temp,
                _ => null
            };
        }

        private async Task GetAPIParametersAsync()
        {
            var options = await General.GetLiveInstanceAsync();
            if (options == null)
            {
                throw new Exception("Please supply required parameters API KEY & URL for the execution");
            }
            apiKey = options.ApiKey ?? throw new Exception("Please supply API KEY for the execution");
            apiUrl = options.ApiUrl ?? throw new Exception("Please supply API URL for the execution");
            if (!string.IsNullOrEmpty(options.Temperature))
            {
                double.TryParse(options.Temperature, out double parsedTemperature);
                temperature = parsedTemperature;
            }
            if (!string.IsNullOrEmpty(options.MaxTokens))
            {
                int.TryParse(options.MaxTokens, out int parsedMaxTokens);
                maxTokens = parsedMaxTokens;
            }
        }
        private async Task<string> OpenAPICallAsync(object requestBody)
        {
            string result = "";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
            using var httpContent = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, httpContent);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic jsonResponse = JsonConvert.DeserializeObject(responseBody);
            //var outputBuilder = new StringBuilder();
            foreach (var choice in jsonResponse.choices)
            {
                result = result + choice.text;
                //outputBuilder.AppendLine(Convert.ToString(choice.text));
            }
            //var output = outputBuilder.ToString().TrimEnd();
            string[] lines = result.Split(new[] { "\r\n", "\n", "\\r\\n", "\\n" }, StringSplitOptions.None);
            var nonEmptyLines = lines.Where(line => !string.IsNullOrEmpty(line) || !string.IsNullOrWhiteSpace(line));
            string outputWithoutEmptyLines = string.Join(Environment.NewLine, nonEmptyLines);
            if (string.IsNullOrEmpty(outputWithoutEmptyLines))
            {
                throw new Exception("There is some error in the Open AI API call");
            }
            return outputWithoutEmptyLines;
        }
        public async Task FormatDocumentAsync(DocumentView docView, object context, string output, string command)
        {
            switch (command)
            {
                case "Suggest":
                case "Tests":
                    int currentLinePosition = (int)context;
                    docView.TextBuffer.Insert(currentLinePosition, output);
                    break;
                case "Optimize":
                    var selection = (Microsoft.VisualStudio.Text.SnapshotSpan)context;
                    docView.TextBuffer.Replace(selection, output);
                    break;
                default:
                    break;
            }

            var syntaxTree = CSharpSyntaxTree.ParseText(docView.TextBuffer.CurrentSnapshot.GetText());
            var formattedNode = Formatter.Format(await syntaxTree.GetRootAsync(), new AdhocWorkspace());
            var formattedCode = formattedNode.ToFullString();
            var span = new Span(0, docView.TextBuffer.CurrentSnapshot.Length);
            var textBuffer = docView.TextBuffer.CurrentSnapshot.TextBuffer;
            var edit = textBuffer.CreateEdit();
            edit.Replace(span, formattedCode);
            edit.Apply();
        }

        public static Boolean CheckFormCall(Form form, string formField)
        {
            if (form != null)
            {
                Type formType = form.GetType();
                var fieldInfo = formType.GetField(formField);
                return fieldInfo != null && fieldInfo.FieldType == typeof(bool);
            }
            return false;
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
        ~Utilities()
        {
            Dispose(false);
        }
    }
}
