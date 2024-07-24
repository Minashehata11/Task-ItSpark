using AutoMapper;
using SchoolSystemStak.DAL.Models;
using SchoolSystemTask.PL.ViewModels;

namespace SchoolSystemTask.PL.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentViewModel>()
                .ForMember(d=>d.ClassName,s=>s.MapFrom(s=>s.Class.Name))
                .ForMember(d=>d.Email,s=>s.MapFrom(s=>s.ApplicationUser.Email));
            CreateMap<CreateOrEditViewModel, Student>().ReverseMap();
            CreateMap<EditViewModel, Student>().ReverseMap();
        }
    }
}
