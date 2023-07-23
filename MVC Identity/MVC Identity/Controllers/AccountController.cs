using Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_First.Models;
using MVC_Identity.Helpers;
using MVC_Identity.ViewModels;

namespace MVC_Identity.Controllers
{
    [Route("[Controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Register")]
        public async Task<IActionResult> Register(string registerUrl = null)
        {
            if (!await _roleManager.RoleExistsAsync("StakeHolder"))
            {
                await _roleManager.CreateAsync(new IdentityRole("StakeHolder"));
                await _roleManager.CreateAsync(new IdentityRole("Developer"));
                await _roleManager.CreateAsync(new IdentityRole("Tester"));
            }
            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                ActionsEmailConfirming = new List<string>()
                {
                    "Now. Send code",
                    "Later"
                }
            };
            foreach (var role in _roleManager.Roles)
            {
                registerViewModel.UserRoles.Add(
                    new SelectListItem($"{role.Name}", $"{role.Name}"));
            }
            registerViewModel.ReturnUrl = registerUrl;
            return View(registerViewModel);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string registerUrl = null)
        {

            registerViewModel.ReturnUrl = registerUrl;
            registerUrl = registerUrl ?? Url.Content("~/");
            if (!ModelState.IsValid||await _userManager.Users.AnyAsync(user=>user.UserName==registerViewModel.UserName))
            {
                return View(registerViewModel);
            }

            var user = new AppUser() { UserName = registerViewModel.UserName, Email = registerViewModel.Email };
            var result = await _userManager.CreateAsync(user, registerViewModel.Password);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Password", "Cannot create user");
            }
            if (registerViewModel.SelectedRole is not null && registerViewModel.SelectedRole.Length > 0)
            {
                await _userManager.AddToRoleAsync(user, registerViewModel.SelectedRole);
            }
            await _signInManager.SignInAsync(user, isPersistent: false);
            switch (registerViewModel.SelectedActionEmailConfirming)
            {
                case "Now. Send code":
                    return RedirectToAction("EmailConfirmation", "Account");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Route("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation()
        {
            var emailConfirmationViewModel = new EmailConfirmationViewModel();
            EmailConfirmationViewModel.CorrectCode = new Random().Next(1000,9999).ToString();
            SendMail.Send((await _userManager.GetUserAsync(User)).Email,"Confirmation", EmailConfirmationViewModel.CorrectCode);
            return View(emailConfirmationViewModel);
        }

        [HttpPost]
        [Route("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation(EmailConfirmationViewModel emailConfirmationViewModel)
        {
            if (emailConfirmationViewModel.VerifyCode==EmailConfirmationViewModel.CorrectCode)
            {
                var user = await _userManager.GetUserAsync(User);
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                    return RedirectToAction("Index", "Home");

            }
            return RedirectToAction("EmailConfirmation", "Account");
        }


        [HttpGet]
        [Route("Login")]
        public IActionResult Login(string registerUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = registerUrl ?? Url.Content("~/");
            return View(loginViewModel);
        }

        [HttpGet]
        [Route("Detail")]
        public async Task<IActionResult> Detail() 
        {
            var user = await _userManager.GetUserAsync(User);
            var detailUserViewModel = new DetailUserViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                Role = (await _userManager.GetRolesAsync(user))[0],
                Projects = user.ProjectUsers.Select(item=>item.Project).ToList()
            };
            return View(detailUserViewModel);
        }

        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string registerUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    loginViewModel.UserName, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);
                //if (result.IsLockedOut) { return View("Lockout", $"You've been locked out. Try in {_signInManager.Options.Lockout.DefaultLockoutTimeSpan.ToString()}"); }
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Invalid login try");
                    return View(loginViewModel);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(loginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword()
        {
            var user = (await _userManager.GetUserAsync(User));
            var forgotPasswordVM = new ForgotPasswordViewModel() {Email = user.Email };
            ForgotPasswordViewModel.CorrectCode = new Random().Next(1000,9999).ToString();
            return View(forgotPasswordVM);
        }
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (forgotPasswordViewModel.Code==ForgotPasswordViewModel.CorrectCode)
            {
                return RedirectToAction("ChangePassword");
            }
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            await _userManager.ChangePasswordAsync(user,user.PasswordHash,changePasswordViewModel.Password);
            return RedirectToAction("Index","Home");
        }
    }
}
