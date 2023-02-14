using OpenAI.GPT3.ObjectModels.ResponseModels.ImageResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.Models.Models
{
    public class ImageResponseModel
    {
        public List<string> ImageUrls { get; set; }

        public int ImageCount { get; set; }
    }
}
