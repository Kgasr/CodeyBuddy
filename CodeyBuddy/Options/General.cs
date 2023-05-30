using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CodeyBuddy
{
    internal partial class OptionsProvider
    {
        // Register the options with this attribute on your package class:
        // [ProvideOptionPage(typeof(OptionsProvider.GeneralOptions), "CodeyBuddy", "General", 0, 0, true, SupportsProfiles = true)]
        [ComVisible(true)]
        public class GeneralOptions : BaseOptionPage<General> { }
    }

    public class General : BaseOptionModel<General>
    {
        [Category("General")]
        [DisplayName("Api Key")]
        [Description("Retrieves the API Key from the extension options")]
        // [DefaultValue(true)]
        public string ApiKey { get; set; }

        [Category("General")]
        [DisplayName("Api Url")]
        [Description("Retrieves the API Url from the extension options")]
        public string ApiUrl { get; set; }

        [Category("Optional")]
        [DisplayName("Max Tokens")]
        [Description("Retrieves the Max Tokens from the extension options")]
        public string MaxTokens { get; set; }

        [Category("Optional")]
        [DisplayName("Temperature")]
        [Description("Retrieves the Temperature Parameters for API Call from the extension options")]
        public string Temperature { get; set; }
    }
}
