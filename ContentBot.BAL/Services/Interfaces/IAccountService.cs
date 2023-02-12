using ContentBot.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.BAL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<APIResponseEntity<RegistrationResponse>> RegisterUser(RegistrationRequestModel registrationRequestModel);

        Task SendEmail(EmailModel emailModel);
    }
}
