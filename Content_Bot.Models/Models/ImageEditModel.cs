using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.Models.Models
{
    public class ImageEditModel
    {
        public IFormFile Image { get; set; }

        public string Instruction { get; set; }

        public int ImageCount { get; set; }

        public string FileName { get; set; }    
    }
}
