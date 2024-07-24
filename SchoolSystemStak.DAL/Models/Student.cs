using SchoolSystemStak.DAL.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystemStak.DAL.Models
{
    public enum Gender
    {
        Male,
        Female
    }
    public enum City
    {
        Alexandria,
        Cairo,
        Domiat,
        Elfayoom
    }

    public class Student:BaseEntity
    {
        [MaxLength(14, ErrorMessage = "Must be 14 numbers only")]
        [MinLength(14,ErrorMessage ="Must be 14 numbers Only")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "Must be 14 numbers only")]
        public string? NationalId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? File { get; set; }
        public Gender Gender { get; set; }
        public City City { get; set; }
        public bool Enrolled { get; set; } = true;
        public string? Remarks { get; set; }
        public Class Class { get; set; }
        public int ClassId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public string? ApplicationUserId { get; set; }
    }
}
