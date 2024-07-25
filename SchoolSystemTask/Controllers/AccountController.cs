using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SchoolSystemStak.DAL.Models.Identity;
using SchoolSystemTask.PL.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


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
            var Login = new SignInViewModel()
            {
                Schemes = await _signInManager.GetExternalAuthenticationSchemesAsync(),
            };
            return View(Login);
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


                var result = await _signInManager.PasswordSignInAsync(user, data.PassWord, true, false);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                  ModelState.AddModelError(string.Empty, "Invalid Username or Password"); 
                    
            }

            return View(data);
        }
        public IActionResult ExternalLogin(string provider, string returnUrl = "")
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "", string remoteError = "")
        {

            var loginVM = new SignInViewModel()
            {
                Schemes = await _signInManager.GetExternalAuthenticationSchemesAsync()
            };

            if (!string.IsNullOrEmpty(remoteError))
            {
                ModelState.AddModelError("", $"Error from extranal login provide: {remoteError}");
                return View("SignIn", loginVM);
            }

            //Get login info
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError("", $"Error from extranal login provide: {remoteError}");
                return View("SignIn", loginVM);
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
                return RedirectToAction("Index", "Home");
            else
            {
                var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (!string.IsNullOrEmpty(userEmail))
                {
                    var user = await _userManager.FindByEmailAsync(userEmail);

                    if (user == null)
                    {
                        user = new ApplicationUser()
                        {
                            UserName = userEmail,
                            Email = userEmail,
                            EmailConfirmed = true
                        };

                        await _userManager.CreateAsync(user);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

            }

            ModelState.AddModelError("", $"Something went wrong");
            return View("SignIn", loginVM);
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }




        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
