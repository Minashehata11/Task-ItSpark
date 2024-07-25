using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace SchoolSystemTask.PL.ViewModels
{
	public class SignInViewModel
	{
		[EmailAddress]
		public string Email { get; set; }
		[DataType(DataType.Password)]
		public string PassWord { get; set; }
        //Thid
        public IEnumerable<AuthenticationScheme> Schemes { get; set; } = new List<AuthenticationScheme>();

    }
}
