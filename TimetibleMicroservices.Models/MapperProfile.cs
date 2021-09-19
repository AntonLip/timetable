using AutoMapper;
using TimetibleMicroservices.Models.DBModels;
using TimetibleMicroservices.Models.DTOModels;

namespace TimetibleMicroservices.Models
{
   public  class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<AddLessonDto, Lesson>();
            CreateMap<Lesson, LessonDto>().ReverseMap();

        }
    }
}
