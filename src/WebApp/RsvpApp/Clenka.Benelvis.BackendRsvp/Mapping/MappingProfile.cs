using AutoMapper;
using Clenka.Benelvis.BackendRsvp.DTOs;
using Clenka.Benelvis.BackendRsvp.Models;

namespace Clenka.Benelvis.BackendRsvp.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RsvpEntityDto,RsvpEntity>().ReverseMap();
            CreateMap<CreateRsvpEntityDto, RsvpEntity>().ReverseMap();
            CreateMap<UpdateRsvpEntityDto, RsvpEntity>().ReverseMap();
        }
    }
}
