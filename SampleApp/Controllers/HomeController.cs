﻿using System;
using MT.Notifications;
using System.Web.Mvc;

namespace SampleApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Script = Notifications.ShowPopup("Hello World! ömer'in", MessageType.Success, "Alert", "TAMAM", 
                new SweetCallBack("alert('Hello World');"));
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.Script = Notifications.ShowToast("HahaHahaha!", string.Empty, MessageType.Warning, position: ToastrPosition.TopCenter);
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.Script = Notifications.ShowGritter("Message", "Title", "https://avatars3.githubusercontent.com/u/10332803?v=3&s=400");
            return View();
        }
    }
}