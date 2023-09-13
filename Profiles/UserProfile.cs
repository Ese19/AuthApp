using AuthApp.Models;
using AutoMapper;

namespace AuthApp.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<Register, User>()
        .ForMember(
            dest => dest.UserName,
            opt => opt.MapFrom(src => $"{src.Name}")
        );
    }
}