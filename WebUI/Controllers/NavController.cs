using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private IThingRepository repository;

        public NavController(IThingRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult Menu(string kind = null)
        {
            ViewBag.SelectedKind = kind;
            IEnumerable<string> kinds = repository.Things
                .Select(thing => thing.Kind)
                .Distinct()
                .OrderBy(x => x);
            

            return PartialView("FlexMenu", kinds);
        }
    }
}