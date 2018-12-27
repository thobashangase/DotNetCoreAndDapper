using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1Dapper.Logic;
using WebApplication1Dapper.Models;

namespace WebApplication1Dapper.Controllers
{
    public class AccountController : Controller
    {
        private readonly PeopleLogic _logic = new PeopleLogic();

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _logic.GetPersonByUsername(model.Username);

                if (result == null)
                {
                    ModelState.AddModelError("", "No such user");
                }
                else
                {
                    var validuser = _logic.ValidateUser(model);
                    if (validuser == null)
                    {
                        ModelState.AddModelError("", "Invalid username/password");
                    }
                    else
                    {
                        //login
                    }
                }
            }
            return View(model);
        }
    }
}