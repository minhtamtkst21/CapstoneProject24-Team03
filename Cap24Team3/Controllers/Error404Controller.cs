﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDangKyKHHT.Controllers
{
    public class Error404Controller : Controller
    {
        // GET: Error404
        public ActionResult Index()
        {
            ViewBag.Title = "Regular Error";
            return View();
        }
        public ActionResult NotFound404()
        {
            ViewBag.Title = "Error 404 - File not Found";
            return View("Index");
        }
    }
}