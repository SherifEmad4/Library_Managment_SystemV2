using AutoMapper;
using Library_Managment.Application.DTOs;
using Library_Managment.Domain.Entities;



namespace Library_Managment.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateBookDto, Book>();
            CreateMap<UpdateBookDto, Book>();
            CreateMap<Member, MemberDto>();
            CreateMap<CreateMemberDto, Member>()
                .ForMember(dest => dest.JoinDate, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }

}
