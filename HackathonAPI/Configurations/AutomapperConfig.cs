using AutoMapper;
using HackathonAPI.Models.AccountModels;
using HackathonAPI.Models;

namespace HackathonAPI.Configurations
{
    public class AutomapperConfig  : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<User, SignUpDTO>().ReverseMap();
        }
    }
}
