using System;
using System.Collections.Generic;
using System.Text;

namespace DebtCollectorQR.Models
{
    public class InvoiceData
    {
        public DateTime CreatedDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string ClientName { get; set; }
        public string ServiceDesc { get; set; }
        public decimal Price { get; set; }
        public string QrCodeFilePath { get; set; }

        public InvoiceData(string clientName, string serviceDescription, decimal price, string qrCodeFilePath)
        {
            ClientName = clientName;
            ServiceDesc = serviceDescription;
            Price = price;
            QrCodeFilePath = qrCodeFilePath;
        }
    }
}
