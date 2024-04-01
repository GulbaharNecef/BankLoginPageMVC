using BankLoginPage.Abstraction.Services.TokenService;
using BankLoginPage.DTOs;
using BankLoginPage.Models;
using BankLoginPage.Models.IdentityEntities;
using BankLoginPage.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Http.Headers;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;


namespace BankLoginPage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        //[Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> Users()
        {
            
            List<AppUser> users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                AppUser user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    UserName = model.Username
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    // If user creation fails, add errors to the ModelState
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }
            else return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (result.Succeeded)
                    {
                        //TokenDTO token = await _tokenHandler.CreateAccessTokenAsync(Int32.Parse(_configuration["Token:AccessTokenLifeTimeInMinutes"]), user);
                        return RedirectToAction("Users");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Username or password is incorrect.");
                        return RedirectToAction("Index");
                    }
                }
                else
                    return View("Istifadeci tapilmadi");
            }
            else
            {
                return View();
            } 

        }
    }
}
