using System.Web.Mvc;
using System.Web.Security;

namespace Punch.Controllers
{
    public class PunchControllerBase : Controller
    {
        public string ReferringController
        {
            get
            {
                var request = Request;
                if (request != null && request.UrlReferrer != null && request.UrlReferrer.Segments.Length >= 1)
                {
                    return request.UrlReferrer.Segments[1].Replace("/", "");
                }
                return string.Empty;
            }
        }


        public string Identity
        {
            set { FormsAuthentication.SetAuthCookie(value, true); }
            get { return User.Identity.Name; }
        }

        public bool IsKnownUser
        {
            get { return string.IsNullOrEmpty(Identity) == false; }
        }

        public ActionResult Reset()
        {
            Identity = null;
            return RedirectToAction("index", "home");
        }
    }
}