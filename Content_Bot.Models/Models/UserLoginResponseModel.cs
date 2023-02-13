using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.Models.Models
{
    public class UserLoginResponseModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
