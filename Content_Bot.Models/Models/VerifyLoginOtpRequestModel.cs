using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.Models.Models
{
    public class VerifyLoginOtpRequestModel
    {
        public string Email { get; set; }
        public string EmailOTP { get; set; }
    }
}
