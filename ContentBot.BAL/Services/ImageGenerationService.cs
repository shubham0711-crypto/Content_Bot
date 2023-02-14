using ContentBot.BAL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.BAL.Services
{
    public class ImageGenerationService : IImageGenerationService
    {
        private readonly IServiceProvider _serviceProvider;

        public ImageGenerationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task GenerateImageFromText(string Text)
        {
            try
            {
                var openAiService = _serviceProvider.GetRequiredService<IOpenAIService>();

                var ImageResult = await openAiService.Image.CreateImage(new ImageCreateRequest
                {
                    Prompt = Text,
                    N = 2,
                    Size = StaticValues.ImageStatics.Size.Size256,
                    ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
                });
            }
            catch(Exception ex) { }


        }
    }
}
