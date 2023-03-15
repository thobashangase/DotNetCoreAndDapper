using Microsoft.AspNetCore.Mvc;
using WebApplication1Dapper.Services;
using WebApplication1Dapper.Models;

namespace WebApplication1Dapper.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IPeopleService _peopleService;

        public PeopleController(IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _peopleService.GetPeopleAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            return View(await _peopleService.GetPersonByIdAsync(id));
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
                    ModelState.AddModelError("", "Problem adding person details");
                }
            }

            //if all else fails and we get here, redisplay form
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            return View(_peopleService.GetPersonByIdAsync(id).Result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] Person model)
        {
            if (ModelState.IsValid)
            {
                if (await _peopleService.UpdatePersonAsync(model) > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Problem updating person details");
                }
            }

            //if all else fails and we get here, redisplay form
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _peopleService.GetPersonByIdAsync(id);

            if (person != null)
            {
                await _peopleService.DeletePersonAsync(person);
                return RedirectToAction("Index");
            }
            
            //if all else fails and we get here, redisplay form
            return View(person);
        }
    }
}