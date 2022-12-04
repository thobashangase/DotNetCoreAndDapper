using Microsoft.AspNetCore.Mvc;
using WebApplication1Dapper.Services;
using WebApplication1Dapper.Models;

namespace WebApplication1Dapper.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPeopleService _peopleService;

        public AccountController(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var validuser = _peopleService.ValidateUserAsync(model);

                if (validuser == null)
                {
                    ModelState.AddModelError("", "Invalid username/password");
                }
                else
                {
                    //login
                }
            }
            return View(model);
        }
    }
}