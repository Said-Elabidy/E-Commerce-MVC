using BusinessLayer.DTO.Account;
using BusinessLayer.Manager.IManager;
using DataAccessLayer.Entities;
using E_Commerce_MVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRegistrationManager _registrationManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IRegistrationManager registrationManager,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            _registrationManager = registrationManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SaveRegister(RegisterVM registerVM)
        {

            if (ModelState.IsValid)
            {
                var RegisterDTO = new RegisterDTO
                {
                    Name = registerVM.UserName,
                    Password = registerVM.Password,
                    Adress = registerVM.Address,
                };

                try
                {
                    await _registrationManager.SaveUserAndCustomer(RegisterDTO);
                    return RedirectToAction("Index", "Home"); 
                }
                catch (Exception ex)
                {
                    
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            
            return View("Register", registerVM);
        }

        public async Task<IActionResult> Logout()
        {
           await _registrationManager.Logout();

            return View("Register");
        }

        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> SaveLogin(LoginVM loginUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginUserViewModel.UserName);
                if (user != null)
                {
                    bool isPasswordMatched = await _userManager
                           .CheckPasswordAsync(user, loginUserViewModel.Password);

                    if (isPasswordMatched == true)
                    {
                        await _signInManager.SignInAsync(user, loginUserViewModel.RememberMe);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "UserName or Password wrong");
                    }
                }

                ModelState.AddModelError("", "UserName or Password wrong");
            }

            return View("Login", loginUserViewModel);
        }
    }
}
