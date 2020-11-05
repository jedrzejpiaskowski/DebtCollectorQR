using System;
using System.Collections.Generic;
using System.Text;

namespace DebtCollectorQR.Models
{
    public class GeneratorOptions
    {
        public string QrDirectory { get; set; }
        public string HtmlTemplatePath { get; set; }
        public string EmailBodyHtmlPath { get; set; }
        public string PdfDirectory { get; set; }
        public int WebPageWidth { get; set; }
        public int WebPageHeight { get; set; }
        public bool CleanupFiles { get; set; }
    }
}
