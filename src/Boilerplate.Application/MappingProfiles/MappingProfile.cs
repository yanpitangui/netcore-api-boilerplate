using AutoMapper;
using Boilerplate.Application.DTOs.Hero;
using Boilerplate.Application.DTOs.User;
using Boilerplate.Domain.Auth;
using Boilerplate.Domain.Entities;

namespace Boilerplate.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Hero Map
            CreateMap<Hero, GetHeroDto>().ReverseMap();
            CreateMap<CreateHeroDto, Hero>();
            CreateMap<UpdateHeroDto, Hero>();

            // User Map
            CreateMap<User, GetUserDto>().ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(x => x.Role == Roles.Admin)).ReverseMap();
            CreateMap<CreateUserDto, User>().ForMember(dest => dest.Role,
                opt => opt.MapFrom(org => org.IsAdmin ? Roles.Admin : Roles.User));
            CreateMap<UpdatePasswordDto, User>();
        }
    }
}
