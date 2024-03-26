using Microsoft.AspNetCore.Mvc;

namespace Ass03.Controllers.userController
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
