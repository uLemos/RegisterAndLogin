using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Identity.Models;

namespace WebApp.Identity.Controllers
{
    //[ApiController] -> Com Razor não é necessário utilizar API
    public class HomeController : Controller
    {
        private readonly UserManager<MyUser> _userManager;

        private readonly ILogger<HomeController> _logger;

        public HomeController(UserManager<MyUser> userManager, ILogger<HomeController> logger)
        {
            this._userManager = userManager;
            this._logger = logger;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        { 
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var identity = new ClaimsIdentity("cookies"); //Adicionando os cookies via Claims
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                    await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(identity));

                    return RedirectToAction("About");
                }

                ModelState.AddModelError("", "Usuário ou Senha Inválida!");
            }
            return View();
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpGet]
        [Authorize] // Se o login não for feito e se não foi armazenado o login, não cai no authentication no startup. Ou seja, essa action irá usar uma authentication.
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null)
                {
                    user = new MyUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);
                }

                return View("Success");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
