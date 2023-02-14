using ContentBot.BAL.Services.Interfaces;
using ContentBot.Models.Models;
using Microsoft.AspNetCore.Http;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;


namespace ContentBot.BAL.Services
{
    public class ImageGenerationService : IImageGenerationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOpenAIService _openAiService;

        public ImageGenerationService(IServiceProvider serviceProvider, IOpenAIService openAiService)
        {
            _serviceProvider = serviceProvider;
            _openAiService = openAiService;
        }

        public async Task<APIResponseEntity<ImageResponseModel>> GenerateImageFromText(string Text)
        {
            APIResponseEntity<ImageResponseModel> response = new();
            ImageResponseModel imageModel = new();

            var ImageResult = await _openAiService.Image.CreateImage(new ImageCreateRequest
            {
                Prompt = Text,
                N = 2,
                Size = StaticValues.ImageStatics.Size.Size256,
                ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
            });

            if (ImageResult.Successful)
            {
                imageModel.ImageUrls = ImageResult.Results.Select(x => x.Url).ToList();
                imageModel.ImageCount = ImageResult.Results.Count;
                response.IsSuccess = true;
                response.Message = "The Generated Imager are:";
                response.Data = imageModel;
                return response;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ImageResult.Error.Message.ToString();
                return response;
            }
        }

        public async Task<APIResponseEntity<ImageResponseModel>> CreateImageEdit(ImageEditModel editModel)
        {
            APIResponseEntity<ImageResponseModel> response = new();
            ImageResponseModel imageModel = new();

            var ms = new MemoryStream();
            await editModel.Image.CopyToAsync(ms);
            var ImageArray = ms.ToArray();

            var ImageResult = await _openAiService.Image.CreateImageEdit(new ImageEditCreateRequest
            {
                Image = ImageArray,
                Prompt = editModel.Instruction,
                ImageName = editModel.FileName,
                N = editModel.ImageCount,
                Size = StaticValues.ImageStatics.Size.Size256,
                ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,

            });

            if (ImageResult.Successful)
            {
                imageModel.ImageUrls = ImageResult.Results.Select(x => x.Url).ToList();
                imageModel.ImageCount = ImageResult.Results.Count;
                response.IsSuccess = true;
                response.Data = imageModel;
                return response;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ImageResult.Error.Message.ToString();
                return response;
            }

        }

        //public async Task<string> SaveImage(IFormFile Image)
        //{
        //    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
        //    var filepath = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);
        //    using (var stream = new FileStream(filepath, FileMode.Create))
        //    {
        //        await Image.CopyToAsync(stream);
        //    }

        //    return filepath;
        //}
    }
}
