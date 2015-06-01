using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusDBWebApplication.Controllers
{
    [RequireHttps]
    [Authorize]
    public class AdminController : Controller
    {
    
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}