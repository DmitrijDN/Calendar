﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using Bs.Calendar.Mvc.Services;

using Bs.Calendar.Mvc.ViewModels;

namespace Bs.Calendar.Mvc.Controllers
{
    public class RoomController : Controller
    {
#warning style
        private RoomService Service;

        public RoomController()
        {
            Service = new RoomService();  /* Инициализация сервиса */
        }

        //
        // GET: /Room/
        public ActionResult List()
        {
            return View(Service.List());
        }

        public ActionResult Index()
        {
            return View(Service.Room);
        }

        //
        // GET: /Room/Save
        public ActionResult Save(RoomEditVm revView)
        {
            Service.Save(revView);

            return View("Index");
        }
    }
}
