using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        IThingRepository repository;

        public AdminController(IThingRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Things);
        }

        public ViewResult Edit(int thingId)
        {
            Thing thing = repository.Things.FirstOrDefault(b => b.ThingId == thingId);
            return View(thing);
        }

        [HttpPost]
        public ActionResult Edit(Thing thing)
        {
            if (ModelState.IsValid)
            {
                repository.SaveThing(thing);
                TempData["message"] = string.Format("Изменение информации о вещи \"{0}\" сохранены", thing.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(thing);
            }
        }

    }
}