using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.Models.Models
{
    public class RegistrationResponse
    {
        public string UserID { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Mobile_No { get; set; }
    }
}
