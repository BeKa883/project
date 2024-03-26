using Ass03.Controllers.ViewModels;
using Ass03.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ass03.Controllers.userController
{
    public class HomeUserController : Controller
    {
        private readonly Ass03Context _context;

        public HomeUserController(Ass03Context context)
        {
            _context = context;
        }

        public IActionResult Index(LoginVM loginVM = null)
        {
            if (loginVM.Username == null)
            {
                var products = _context.Products.Include(p => p.Category).ToList();
                return View(products);
            }
            else
            {
                var products = _context.Products.Include(p => p.Category).ToList();
                ViewData["Username"] = loginVM.Username;
                return View(products);
            }
           
        }
    }
}
