using Microsoft.AspNetCore.Mvc;

namespace EPR.PRN.Backend.API.Controllers
{
    public class PrnController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
