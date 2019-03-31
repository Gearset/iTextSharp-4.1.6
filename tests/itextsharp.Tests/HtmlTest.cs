using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Xunit;
using Xunit.Abstractions;

namespace itextsharp.Tests
{
    public class HtmlTest
    {
        private readonly ITestOutputHelper _output;

        public HtmlTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void CanGenerateHtml()
        {
            const string c_AssetFolderName = ".PdfResources";

            // Load roboto font for later use.
            var assembly = Assembly.GetExecutingAssembly();
            var resoureNamespace = GetType().Namespace + c_AssetFolderName;
            LoadPdfFont(assembly, "Roboto.ttf", resoureNamespace);
            
            // Get report html from embedded resources
            var htmlBytes = GetResource(assembly, resoureNamespace + ".report.html");
            var html = new StringBuilder(Encoding.UTF8.GetString(htmlBytes));

            // Load html string with actual deployment data
            var differences = Enumerable.Range(0, 100).Select(x => new Row());

            html.Replace("{DEPLOY_MESSAGE}","The deployment finished at <strong>{DEPLOY_END_TIME}</strong>.");

            html.Replace("{DEPLOY_START}", "N/A");
            html.Replace("{DEPLOY_END}", "N/A");
            html.Replace("{DEPLOY_START_TIME}", "N/A");
            html.Replace("{DEPLOY_END_TIME}", "N/A");
            html.Replace("{DEPLOY_DURATION}", "N/A");

            html.Replace("{ORG_SOURCE_NAME}", "Unknown");
            html.Replace("{ORG_SOURCE_TYPE}", "N/A");

            html.Replace("{ORG_TARGET_NAME}", "Unknown");
            html.Replace("{ORG_TARGET_TYPE}", "N/A");

            html.Replace("{DEPLOY_OBJ_TOTAL}", "10");
            html.Replace("{DEPLOY_OBJ_CHANGED}", "100");
            html.Replace("{DEPLOY_OBJ_CREATED}", "1000");
            html.Replace("{DEPLOY_OBJ_DELETED}", "10000");

            html.Replace("{TESTS_RUN}", "5");
            html.Replace("{TESTS_PASSED}", "50");
            html.Replace("{TESTS_TIME}", "1 hour 30 min");

            html.Replace("{DEPLOY_OBJ_DETAILS}", String.Join("\n", HtmlPdfColumnHeaders.Concat(differences.Select(FormatRowAsHtml))));

            var deploymentFriendlyName = string.Empty;
            html.Replace("{DEPLOYMENT_FRIENDLY_NAME}", deploymentFriendlyName);
            var annotationText = "<i>No deployment notes attached.</i>";
            html.Replace("{DEPLOYMENT_NOTES}", annotationText);
            var jiraTicketsText = "<i>No associated Jira tickets.</i>";
            html.Replace("{JIRA_TICKETS}", jiraTicketsText);

            var canGenerateHtml = HtmlToPdf(html.ToString());
            
            var tempFileName = Path.GetTempFileName() + ".pdf";
            File.WriteAllBytes(tempFileName, canGenerateHtml);

            _output.WriteLine($"{tempFileName} contains the pdf");

            if (Debugger.IsAttached)
            {
                Process.Start(new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = tempFileName
                });
            }
        }
        
        private static string FormatRowAsHtml(Row comparisonTableRow)
        {
            return $"<tr><td>{comparisonTableRow.ObjectType}</td><td>{comparisonTableRow.DifferenceType}</td><td>{comparisonTableRow.DisplayName}</td>";
        }
        
        private static IEnumerable<string> HtmlPdfColumnHeaders => new[] { "<tr><td style=\"font-weight: bold\">Metadata type</td><td style=\"font-weight: bold\">Difference type</td><td style=\"font-weight: bold\">Name</td></tr>" };

        private static byte[] HtmlToPdf(string html)
        {
            using (var mStream = new MemoryStream())
            {
                // The ordering of these calls is important, as iTextSharp relies on side-effects :(
                var document = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter.GetInstance(document, mStream);
                document.Open();
                var parsedHtml = HTMLWorker.ParseToList(new StringReader(html), null);
                foreach (var element in parsedHtml)
                    document.Add(element as IElement);
                document.Close();
                return mStream.ToArray();
            }
        }

        private static void LoadPdfFont(Assembly assembly, string fontFilename, string fontNamespace)
        {
            if (PdfResourceFileCache.ContainsKey(fontFilename)) return;
            var font = GetResource(assembly, fontNamespace + "." + fontFilename);
            PdfResourceFileCache.Set(fontFilename, font);
        }

        private static byte[] GetResource(Assembly assembly, string resourceName)
        {
            byte[] buffer;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }
    }

    internal class Row
    {
        public string ObjectType => Guid.NewGuid().ToString();
        public string DifferenceType => Guid.NewGuid().ToString();
        public string DisplayName => Guid.NewGuid().ToString();
    }
}