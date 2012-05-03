using System.Web.Mvc;

namespace Punch.Controllers
{
    public class NullPartialViewResult : PartialViewResult
    {
        public NullPartialViewResult()
        {
            var partialViewResult = new PartialViewResult();
            
        }
    }
}