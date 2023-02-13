using AutoMapper;
using ContentBot.DAL.Entities;
using ContentBot.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.BAL.MapperProfile
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<ApplicationUser, RegistrationRequestModel>().ReverseMap();    

        }

    }
}
