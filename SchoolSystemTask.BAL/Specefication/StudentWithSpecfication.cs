using SchoolSystemStak.DAL.Models;

namespace SchoolSystemTask.BAL.Specefication
{
    public class StudentsWithSpecification : SpeceficationBase<Student>
    {
        public StudentsWithSpecification(SpecParameter parameter) :
            base(s =>
                (! (parameter.ClassId.HasValue) || s.ClassId == parameter.ClassId)  &&
                (string.IsNullOrEmpty(parameter.SearchByName)) ||s.Name.ToLower().Trim().Contains(parameter.SearchByName) )
            
        {
            AddInclude(s => s.ApplicationUser);
            AddInclude(s => s.Class);
        }

        public StudentsWithSpecification(int id) :
           base(s => s.Id == id)
        {
            AddInclude(s => s.ApplicationUser);
            AddInclude(s => s.Class);
        }
    }
}
