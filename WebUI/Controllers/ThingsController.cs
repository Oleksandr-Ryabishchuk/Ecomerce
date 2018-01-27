using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ThingsController : Controller
    {
        private IThingRepository repository;
        public int pageSize = 4;
        public ThingsController(IThingRepository repo)
        {
            repository = repo;
        }
        public ViewResult List(string kind, int page = 1)
        {
            ThingsListViewModel model = new ThingsListViewModel
            {
                Things = repository.Things
                .Where(b => kind == null||b.Kind == kind)
                .OrderBy(thing => thing.ThingId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = kind == null 
                         ? repository.Things.Count() :
                         repository.Things.Where(thing =>thing.Kind == kind).Count() 
                },
                CurrentKind = kind
        };
            return View(model);
        }
        
    }
}