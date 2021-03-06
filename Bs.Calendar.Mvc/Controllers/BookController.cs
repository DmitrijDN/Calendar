﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bs.Calendar.Models;
using Bs.Calendar.Mvc.Services;
using Bs.Calendar.Mvc.ViewModels;
using Microsoft.Ajax.Utilities;

namespace Bs.Calendar.Mvc.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService _service;

        public BookController(BookService service)
        {
            _service = service;
        }

        public ActionResult Edit(BookHistoryVm model, HttpPostedFileBase image)
        {
            if (model.BookId == 0)
            {
                ModelState.Remove("BookId");
                if (ModelState.IsValid)
                {                    
                    try
                    {
                        _service.Save(model);
                    }
                    catch (WarningException exception)
                    {
                        ModelState.AddModelError("", exception.Message);
                        return View("Edit", model);
                    }
                }
            }
            if (!ModelState.IsValid)
            {
                return View("Edit", model);
            }
            if (image != null)
            {
                var path = Path.Combine(Server.MapPath("~/Images/Books"),
                    string.Format("{0}.{1}", model.BookCode, "jpg"));                
                image.SaveAs(path);
                _service.AddCover(model.BookCode);
            }
            return model.BookId == 0
                ? RedirectToAction("Index")
                : RedirectToAction("Edit", new { @id = _service.Get(model.BookCode).Id });
        }

        public ActionResult Get(int id)
        {
            var book = _service.Get(id);
            if (book != null)
            {
                return Json(book, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        public JsonResult ListAllBooks()
        {
            var books = _service.GetAllBooks();
            return Json(books, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBookByTags(string tags)
        {
            var books = _service.GetBookByTags(tags);
            return Json(books, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetBookTags(int bookId)
        //{
        //    var res = _service.GetBookTags(bookId);
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult ListSearch()
        {
            var orderby = Request["orderby"];
            var searchStr = Request["search"];
            var books = _service.Load(orderby, searchStr);
            //var page = Request["page"];
            //int pageNumber;
            //try
            //{
            //    pageNumber = Convert.ToInt32(page);
            //}
            //catch
            //{
            //    pageNumber = 0;
            //}
            //if (pageNumber < 1)
            //{
            //    return Json(books, JsonRequestBehavior.AllowGet);
            //}
            var pager = new GenericPagingVm<Book>(books, 1, books.Count());
            return Json(pager, JsonRequestBehavior.AllowGet);
        }

        public JsonResult List()
        {
            var orderby = Request["orderby"];
            var searchStr = Request["search"];
            var books = _service.Load(orderby, searchStr);
            var page = Request["page"];
            int pageNumber;
            try
            {
                pageNumber = Convert.ToInt32(page);
            }
            catch
            {
                pageNumber = 0;
            }
            if (pageNumber < 1)
            {
                return Json(books, JsonRequestBehavior.AllowGet);
            }
            var pager = new GenericPagingVm<Book>(books, pageNumber);
            return Json(pager, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View("Edit", null);
        }

        [HttpPost,
         ValidateAntiForgeryToken]
        public ActionResult Create(BookHistoryVm book)
        {
            ModelState.Remove("BookId");
            if (ModelState.IsValid)
            {
                try
                {
                    _service.Save(book);
                    return RedirectToAction("Index");
                }
                catch (WarningException exception)
                {
                    ModelState.AddModelError("", exception.Message);
                    return View("Edit", book);
                }
            }
            return View("Edit", book);
        }

        [HttpGet]
        [OutputCache(VaryByParam = "*", NoStore = true, Duration = 0)]
        public ActionResult Edit(int id)
        {
            try
            {
                return View("Edit", _service.GetBookHistories(id));
            }
            catch (WarningException)
            {
                return HttpNotFound();
            }
        }

        public ActionResult Save(BookHistoryVm book)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", book);            
            }
            try
            {
                _service.Save(book);
            }
            catch (WarningException exception)
            {
                ModelState.AddModelError("", exception.Message);
            }
            return Json(new { redirectToUrl = Url.Action("Edit"), id = _service.Get(book.BookCode).Id });
        }

        public ActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
            }
            catch (ArgumentException)
            {
                return HttpNotFound();
            }

            return RedirectToAction("Index");
        }
    }
}
