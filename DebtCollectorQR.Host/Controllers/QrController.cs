using DebtCollectorQR.Api.Messages;
using DebtCollectorQR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace DebtCollectorQR.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QrController : ControllerBase
    {
        private readonly InvoiceGenerator _invoiceGenerator;
        private readonly PaymentDataGenerator _paymentDataGenerator;
        private readonly SmtpEmailFileSender _emailSender;
        private readonly GeneratorSensitiveOptions _sensitiveOpts;
        private readonly GeneratorOptions _genOpts;

        public QrController(InvoiceGenerator invoiceGenerator, PaymentDataGenerator paymentDataGenerator, SmtpEmailFileSender emailSender, IOptions<GeneratorOptions> genOptions, IOptions<GeneratorSensitiveOptions> genSensitiveOptions)
        {
            this._invoiceGenerator = invoiceGenerator;
            this._paymentDataGenerator = paymentDataGenerator;
            this._emailSender = emailSender;
            this._sensitiveOpts = genSensitiveOptions.Value;
            this._genOpts = genOptions.Value;
        }

        [HttpPost("[action]")]
        public BaseResponse GenerateRequest(GeneratorRequest request)
        {
            var response = new BaseResponse();
            try
            {
                var qr = _paymentDataGenerator.GenerateQR(_sensitiveOpts.AccountOwner, _sensitiveOpts.AccountNo, request.Price, request.ServiceDescription);

                if (request.SendInvoice)
                {
                    var invoiceData = new InvoiceData(request.Buyer, request.ServiceDescription, request.Price, qr.FullName);
                    var pdf = _invoiceGenerator.GenerateInvoicePdf(invoiceData);
                    string emailBody = "email body";
                    if (!string.IsNullOrEmpty(_genOpts.EmailBodyHtmlPath) && System.IO.File.Exists(_genOpts.EmailBodyHtmlPath))
                    {
                        emailBody = System.IO.File.ReadAllText(_genOpts.EmailBodyHtmlPath);

                    }
                    //_emailSender.SendFile(request.BuyerEmail, $"Fakturka za {request.ServiceDescription}", emailBody, pdf, qr);
                    if (_genOpts.CleanupFiles)
                    {
                        System.IO.File.Delete(qr.FullName);
                        System.IO.File.Delete(pdf.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}
