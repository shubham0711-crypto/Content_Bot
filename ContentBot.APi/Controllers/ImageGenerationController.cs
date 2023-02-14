using ContentBot.BAL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContentBot.APi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageGenerationController : ControllerBase
    {
        private readonly IImageGenerationService _imageGenerationService;

        public ImageGenerationController(IImageGenerationService imageGenerationService)
        {
            _imageGenerationService = imageGenerationService;
        }

        [HttpGet, Route("generateImage")]
        public async Task GenerateImage(string Text)
        {
          await  _imageGenerationService.GenerateImageFromText(Text);
        }
    }
}
