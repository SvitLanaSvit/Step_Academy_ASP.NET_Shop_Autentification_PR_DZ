﻿using ASP_Meeting_18.Data;
using ASP_Meeting_18.Models.DTOs.UserDTOs;
using AutoMapper;

namespace ASP_Meeting_18.AutoMapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<User, UserDTO>().ForMember(dest=>dest.Login, opt=>opt.MapFrom(src=>src.UserName)).ReverseMap();
            CreateMap<User, EditUserDTO>().ForMember(dest=>dest.Login, opt=>opt.MapFrom(src=>src.UserName)).ReverseMap();
            CreateMap<User, CreateUserDTO>().ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName)).ReverseMap();
            CreateMap<User, DeleteUserDTO>().ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName)).ReverseMap();
            CreateMap<User, ChangePasswordDTO>().ReverseMap();
        }
    }
}
