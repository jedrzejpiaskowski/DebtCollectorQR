using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtCollectorQR.Api.Messages
{
    public class GeneratorRequest
    {
        public string Buyer { get; set; }
        public string BuyerEmail { get; set; }
        public decimal Price { get; set; }
        public string ServiceDescription { get; set; }
        public bool SendInvoice { get; set; }
    }
}
