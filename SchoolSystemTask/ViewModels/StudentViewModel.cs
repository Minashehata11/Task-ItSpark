using SchoolSystemStak.DAL.Models.Identity;
using SchoolSystemStak.DAL.Models;

namespace SchoolSystemTask.PL.ViewModels
{
    public class StudentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public City City { get; set; }
        public string ClassName { get; set; }
        public string Email { get; set; }
        public bool Enrolled { get; set; }
    }
}
