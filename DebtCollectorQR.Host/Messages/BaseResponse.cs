using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtCollectorQR.Api.Messages
{
    public class BaseResponse
    {
        public string ErrorMessage { get; set; }
        public bool IsOk => string.IsNullOrEmpty(ErrorMessage);
    }
}
