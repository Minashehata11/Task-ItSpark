using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SchoolSystemStak.DAL.Models.Identity;
using SchoolSystemTask.PL.ViewModels;

namespace SchoolSystemTask.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IToastNotification _toastNotification;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IToastNotification toastNotification)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _toastNotification = toastNotification;
        }

        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            return View(new SignUpViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel data)
        {
            if (ModelState.IsValid)
            {
                // Create a new ApplicationUser with data from the view model
                var user = new ApplicationUser()
                {
                    Email = data.Email,
                    UserName = data.Email.Split("@")[0],
                };

                // Create the user using UserManager
                var result = await _userManager.CreateAsync(user, data.PassWord);

                if (!result.Succeeded)
                {

                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }

                    return View(data);
                }

                _toastNotification.AddSuccessToastMessage("Register Has Confirmed");
                   return RedirectToAction("SignIn");
                   
                    
               
            }

            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> SignIn()
        {
            return View(new SignInViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel data)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(data.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid Username or Password"); 
                    return View(data);
                }

               
                var result = await _signInManager.CheckPasswordSignInAsync(user, data.PassWord,true);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                  ModelState.AddModelError(string.Empty, "Invalid Username or Password"); 
                    
            }

            return View(data);
        }
    }
}
