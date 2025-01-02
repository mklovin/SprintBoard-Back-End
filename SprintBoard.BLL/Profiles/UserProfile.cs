using AutoMapper;
using SprintBoard.DTOs;
using SprintBoard.Entities;

namespace SprintBoard.BLL.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();

        }
    }
}
