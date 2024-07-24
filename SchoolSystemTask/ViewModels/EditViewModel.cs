using SchoolSystemStak.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystemTask.PL.ViewModels
{
    public class EditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [MaxLength(14, ErrorMessage = "Must be 14 numbers only")]
        [MinLength(14, ErrorMessage = "Must be 14 numbers Only")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "Must be 14 numbers only")]
        public string? NationalId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public IFormFile? FileImage { get; set; }
        public bool Enrolled { get; set; } = true;
        public string? Remarks { get; set; }
        public int ClassId { get; set; }
   
    }
}
