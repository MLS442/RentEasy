using AutoMapper;
using RentEasyAPI.DTOs;
using RentEasyAPI.Models;

namespace RentEasyAPI.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserRegisterDto, Landlord>();
            CreateMap<UserRegisterDto, Tenant>();
        }
    }
}
