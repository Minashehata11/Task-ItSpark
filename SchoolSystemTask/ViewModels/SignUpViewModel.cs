using System.ComponentModel.DataAnnotations;

namespace SchoolSystemTask.PL.ViewModels
{
    public class SignUpViewModel
    {
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string PassWord { get; set; }

		[Compare(nameof(PassWord), ErrorMessage = "Dont match")] 
        public string ConfirmPassWord { get; set; }
    }
}
