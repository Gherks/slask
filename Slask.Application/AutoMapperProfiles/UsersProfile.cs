using AutoMapper;
using Slask.Domain;
using Slask.Dto;

namespace Slask.Application.AutoMapperProfiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
