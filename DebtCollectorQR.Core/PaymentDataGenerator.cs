using DebtCollectorQR.Models;
using Microsoft.Extensions.Options;
using QRCoder;
using System;
using System.Drawing;
using System.IO;

namespace DebtCollectorQR
{
    public class PaymentDataGenerator
    {
        private readonly GeneratorOptions _genOptions;

        public PaymentDataGenerator(IOptions<GeneratorOptions> options)
        {
            this._genOptions = options.Value;
        }

        public FileInfo GenerateQR(string recipient, string accountNo, decimal amount, string title)
        {
            string paymentString = GetPaymentDataString(recipient, accountNo, amount, title);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(paymentString, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(10);

            if (!Directory.Exists(_genOptions.QrDirectory))
            {
                Directory.CreateDirectory(_genOptions.QrDirectory);
            }
            var now = DateTime.Now;
            string qrFileName = $"qr_{now.Year}-{now.Month}-{now.Day}_{now.Hour}_{now.Minute}_{now.Second}.png";
            qrFileName = Path.Combine(_genOptions.QrDirectory, qrFileName);
            qrCodeImage.Save(qrFileName);
            return new FileInfo(qrFileName);
        }

        public string GetPaymentDataString(string recipient, string accountNo, decimal amount, string title)
        {
            int amountFull = (int)(amount * 100);
            return $"||{accountNo}|{amountFull:D6}|{recipient}|{title}|||";
        }
    }
}
