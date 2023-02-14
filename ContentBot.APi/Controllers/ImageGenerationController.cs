using ContentBot.BAL.Services.Interfaces;
using ContentBot.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<IActionResult> GenerateImage(string Text)
        {
            APIResponseEntity<ImageResponseModel> response = new();
            try
            {
                response = await _imageGenerationService.GenerateImageFromText(Text);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
        [HttpPost, Route("editImage")]
        public async Task<IActionResult> CreateEditImage([FromForm] ImageEditModel model)
        {
            APIResponseEntity<ImageResponseModel> response = new();
            try
            {
                response = await _imageGenerationService.CreateImageEdit(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Code = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
