using DebtCollectorQR.Models;
using Microsoft.Extensions.Options;
using SelectPdf;
using System;
using System.IO;

namespace DebtCollectorQR
{
    public class InvoiceGenerator
    {
        private readonly GeneratorOptions _options;

        public InvoiceGenerator(IOptions<GeneratorOptions> options)
        {
            this._options = options.Value;
        }

        public FileInfo GenerateInvoicePdf(InvoiceData data)
        {
            string html = GetInvoiceHtml(data);

            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.WebPageWidth = converter.Options.WebPageHeight = 100;

            if (!Directory.Exists(_options.PdfDirectory))
            {
                Directory.CreateDirectory(_options.PdfDirectory);
            }
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(html, _options.HtmlTemplatePath);
            string pdfPath = Path.Combine(_options.PdfDirectory, GetPdfName());
            doc.Save(pdfPath);
            doc.Close();

            return new FileInfo(pdfPath);
        }

        public string GetInvoiceHtml(InvoiceData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrEmpty(data.ServiceDesc))
                throw new ArgumentNullException(nameof(data.ServiceDesc));

            if (string.IsNullOrEmpty(data.ClientName))
                throw new ArgumentNullException(nameof(data.ClientName));

            if (string.IsNullOrEmpty(data.QrCodeFilePath))
                throw new ArgumentNullException(nameof(data.QrCodeFilePath));

            string htmlTemplate = File.ReadAllText(_options.HtmlTemplatePath);
            data.InvoiceNumber = GetInvoiceNumber();
            var createdDate = DateTime.Now;

            string invoiceHtml = htmlTemplate
                .Replace($"@@{nameof(InvoiceData.ClientName)}@@", data.ClientName)
                .Replace($"@@{nameof(InvoiceData.ServiceDesc)}@@", data.ServiceDesc)
                .Replace($"@@{nameof(InvoiceData.Price)}@@", data.Price.ToString())
                .Replace($"@@{nameof(InvoiceData.CreatedDate)}@@", GetShortDate(createdDate))
                .Replace($"@@{nameof(InvoiceData.InvoiceNumber)}@@", data.InvoiceNumber)
                .Replace($"@@{nameof(InvoiceData.QrCodeFilePath)}@@", data.QrCodeFilePath)
                .Replace($"@@{nameof(InvoiceData.PaymentDate)}@@", GetShortDateAndTime(createdDate.AddMinutes(10)));
            return invoiceHtml;
        }

        private string GetInvoiceNumber()
        {
            var n = DateTime.Now;
            var todaySpan = n - n.Date;
            return $"JP/{n.Year}/{n.Month}/{n.Day}/{(long)todaySpan.TotalMilliseconds}";
        }

        private string GetShortDate(DateTime date)
        {
            return $"{date.Day}.{date.Month}.{date.Year}";
        }

        private string GetShortDateAndTime(DateTime dt)
        {
            return $"{dt.Day}.{dt.Month}.{dt.Year} {dt.Hour}:{dt.Minute:D2}";
        }

        private string GetPdfName(string suffix = "")
        {
            var n = DateTime.Now;
            return $"pdf_{n.Year}-{n.Month}-{n.Day}_{n.Hour}-{n.Minute}-{n.Second}-{n.Millisecond}{suffix}.pdf";
        }
    }
}
