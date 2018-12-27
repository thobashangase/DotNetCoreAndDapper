using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1Dapper.Logic;
using WebApplication1Dapper.Models;

namespace WebApplication1Dapper.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IPeopleLogic _logic;

        public PeopleController(IPeopleLogic logic)
        {
            _logic = logic;
        }

        public IActionResult Index()
        {
            return View(_logic.GetPeople());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm] Person model)
        {
            if (ModelState.IsValid)
            {
                if (_logic.AddPerson(model) > 0)
                {
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            //if all else fails and we get here, redisplay form
            return View(model);
        }
    }
}