using AutoMapper;
using DoubleV.DTOs;
using DoubleV;
using DoubleV.Models;

namespace DoubleV.Mapping
{
    public class MappingTarea : AutoMapper.Profile
    {
        public MappingTarea()
        {
            CreateMap<TodoWithoutIdDTO, Todo>()
                .ForMember<User>(dest => dest.User, opt => opt.Ignore());

            CreateMap<Todo, TodoWithoutIdDTO>();

            //CreateMap<TodoUpdateDTO, Todo>()
            //.ForMember(dest => dest.TodoId, opt => opt.Ignore()) // Ignore TodoId to not changing the Id
            //.ForMember<User>(dest => dest.User, opt => opt.Ignore());          
        }        
    }
}
