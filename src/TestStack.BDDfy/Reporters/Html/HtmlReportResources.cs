using System.IO;

namespace TestStack.BDDfy.Reporters.Html
{
    internal class HtmlReportResources
    {
        public static string metro_js_min => Read("TestStack.BDDfy.Reporters.Html.Scripts.metro.min.js");
        public static string metro_css_min => Read("TestStack.BDDfy.Reporters.Html.Scripts.metro.min.css");
        public static string classic_js_min => Read("TestStack.BDDfy.Reporters.Html.Scripts.classic.min.js");
        public static string jquery_2_1_0_min => Read("TestStack.BDDfy.Reporters.Html.Scripts.jquery-2.1.0.min.js");
        public static string classic_css_min => Read("TestStack.BDDfy.Reporters.Html.Scripts.classic.min.css");

        public static string CustomStylesheetComment =>
            "If you drop a custom stylesheet named BDDfyCustom.css in your output folder" +
            " it gets embedded here. This way you can apply some custom styles over your" +
            " html report.";

        public static string CustomJavascriptComment =>
            "If you drop a custom Javascript named BDDfyCustom.js in your output folder" +
            " it gets embedded here. This way you can apply some custom Javascript logic" +
            " to your html report.";

        static string Read(string resourceName)
        {
            var assembly = typeof(HtmlReportResources).Assembly();

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}