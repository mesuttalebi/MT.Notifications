using System;
using MT.Notifications;
using System.Web.Mvc;

namespace SampleApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Script = Notifications.ShowPopup("Hello World!", MessageType.Success, "Alert", "OK",
                new SweetCallBack("alert('Hello World');") );
            
            return View();
        }

        public ActionResult ShowConfirm()
        {
            ViewBag.Script = Notifications.ShowConfirm("This is Confirm Message are you sure?"
                , new SweetCallBack(Notifications.ShowPopup("Confirmed!", MessageType.Success, false), Notifications.ShowErrorToast("Canceled!", "Toastr.js", false) )
                , "Title"
                , "OkText"
                , "CancelText"
                , MessageType.Question
                , true);

            return View("Index");
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