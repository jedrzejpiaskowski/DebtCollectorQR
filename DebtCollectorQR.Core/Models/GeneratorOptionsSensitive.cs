using System;
using System.Collections.Generic;
using System.Text;

namespace DebtCollectorQR.Models
{
    public class GeneratorSensitiveOptions
    {
        public string SmtpEmailHost { get; set; }
        public int SmtpPort { get; set; }
        public string EmailUsername { get; set; }
        public string EmailPassword { get; set; }
        public string EmailFromDisplayName { get; set; }
        public string AccountNo { get; set; }
        public string AccountOwner { get; set; }
    }
}
