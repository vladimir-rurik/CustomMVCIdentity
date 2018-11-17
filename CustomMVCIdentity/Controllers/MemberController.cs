using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomMVCIdentity.Filters;

namespace CustomMVCIdentity.Controllers
{
    [Authorize(Roles = "Member")]
    [VerifyUser]
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult Index()
        {
            return View();
        }
    }
}