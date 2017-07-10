using MT.Notifications;
using System.Web.Mvc;

namespace SampleApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Script = Notifications.ShowPopup("Hello World!", MessageType.Success, callbacks: new SweetCallBack("alert('Hello World');"), showLoaderOnConfirm: true);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.Script = Notifications.ShowToast("HahaHahaha!", MessageType.Warning, position: ToastrPosition.TopCenter);
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