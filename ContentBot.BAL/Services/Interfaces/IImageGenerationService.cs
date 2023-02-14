using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.BAL.Services.Interfaces
{
    public interface IImageGenerationService
    {
        Task GenerateImageFromText(string Text);

    }
}
