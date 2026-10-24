using AutoMapper;
using RentEasyAPI.DTOs;
using RentEasyAPI.Models;
using RentEasyAPI.Responses;

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
