using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1Dapper.Services;
using WebApplication1Dapper.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;

namespace WebApplication1Dapper.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IPeopleService _peopleService;

        public PeopleController(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _peopleService.GetPeopleAsync());
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Person model)
        {
            if (ModelState.IsValid)
            {
                if (await _peopleService.AddPersonAsync(model) > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Problem adding a person");
                }
            }
            //if all else fails and we get here, redisplay form
            return View(model);
        }
    }
}