using DoubleV.DTOs;
using DoubleV.Models;

namespace DoubleV.Mapping
{
    public class MappingUsuario : AutoMapper.Profile
    {
        public MappingUsuario()
        {
            CreateMap<User, UserWithoutIdDTO>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
             .ForMember(dest => dest.RolId, opt => opt.MapFrom(src => src.RolId))
             .ReverseMap();

            CreateMap<LoginDTO, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Name, opt => opt.Ignore()) 
            .ForMember(dest => dest.RolId, opt => opt.Ignore());
        }
    }
}
