using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.Models.Models
{
    public class EmailSettings
    {
        public string EmailFrom { get; set; }
        public string SMTPUsername { get; set; }
        public string SMTPPassword { get; set; }
        public int Port { get; set; }
        public string SmtpHost { get; set; }

    }
}
