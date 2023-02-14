using ContentBot.Models.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.BAL.Services.Interfaces
{
    public interface IImageGenerationService
    {
        Task<APIResponseEntity<ImageResponseModel>> GenerateImageFromText(string Text);

        Task<APIResponseEntity<ImageResponseModel>> CreateImageEdit(ImageEditModel editModel);



    }
}
