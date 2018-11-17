using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomMVCIdentity.Filters;

namespace CustomMVCIdentity.Controllers
{
    [Authorize(Roles = "Administrator")]
    [VerifyUser]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}