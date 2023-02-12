using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.Models.Models
{
    public class APIResponseEntity<T>
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public bool IsSuccess { get; set; } = true;
    }
}
