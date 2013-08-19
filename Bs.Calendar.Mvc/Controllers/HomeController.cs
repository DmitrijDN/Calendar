﻿using System;
using System.Web.Mvc;
using Bs.Calendar.Mvc.Services;
using Bs.Calendar.Mvc.ViewModels.Home;

namespace Bs.Calendar.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _service;

        public HomeController(HomeService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            var users = _service.LoadUsers();
            return View(users);
        }

        public ActionResult CreateEvent()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetEvents(DateTime from, DateTime to)
        {
            return Json(_service.GetEvents(from, to), JsonRequestBehavior.AllowGet);
        }

        //TODO: Replace this thing with something appropriate
        public ActionResult Edit()
        {
            return View("Edit");
        }
    }
}
