using Ass03.Controllers.ViewModels;
using Ass03.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ass03.Controllers.userController
{
    public class LoginController : Controller
    {
        private readonly Ass03Context _context;

        public LoginController(Ass03Context context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            LoginVM loginVM = new LoginVM();
            return View(loginVM);
        }

        public IActionResult Log_out()
        {
           
                HttpContext.Session.Clear();
                LoginVM loginVM = new LoginVM();
                return RedirectToAction(nameof(Index), "HomeUser",loginVM);
            
           
        }

        [HttpPost]
        public IActionResult Login(LoginVM loginVM)
        {
           
            User u2 = _context.Users.Where(u => u.Username == loginVM.Username).FirstOrDefault();
            if (u2.Password == loginVM.Password)
            {
                
                if (u2.RoleId == 1)
                {
                    HttpContext.Session.SetString("userid", u2.UserId.ToString());
                    HttpContext.Session.SetString("username", u2.Username);
                    HttpContext.Session.SetString("role", u2.RoleId.ToString());
                    return RedirectToAction(nameof(Index), "Home",u2);
                }
                else
                {
                    var cart = _context.Carts.FirstOrDefault(c => c.UserId == u2.UserId);
                    HttpContext.Session.SetString("userid", u2.UserId.ToString());
                    HttpContext.Session.SetString("role", u2.RoleId.ToString());
                    if (cart != null) // Kiểm tra nếu có giỏ hàng
                    {
                        HttpContext.Session.SetString("cartid", cart.CartId.ToString()); // Gán CartId vào session
                    }
                    HttpContext.Session.SetString("username", u2.Username);
                    return RedirectToAction(nameof(Index), "HomeUser", u2);

                }
            }
            else
            {
                ModelState.AddModelError("Username", "Invalid username or password");
                return View(nameof(Login));

            }




        }

        public IActionResult Register()
        {
            RegisterVM registerVM = new RegisterVM();
            return View(registerVM);
        }

        [HttpPost]
        public IActionResult Register(RegisterVM registerVM)
        {
            User u1 = _context.Users.Where(u => u.Username == registerVM.Username).FirstOrDefault();
            if(u1 != null)
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(registerVM);
            }
            if(registerVM.Password == registerVM.ConfirmPassword)
            {
                User u = new User()
                {
                    Username = registerVM.Username,
                    Password = registerVM.Password,
                    RoleId = 2,
                    Status = 1,
                };
                _context.Users.Add(u);
                _context.SaveChanges();
                // Tạo một Cart mới cho User vừa được tạo
                Cart newCart = new Cart()
                {
                    UserId = u.UserId // Sử dụng ID của User mới tạo
                };
                _context.Carts.Add(newCart);
                _context.SaveChanges();
                return RedirectToAction(nameof(Login), "Login");
            }
            else
            {
                ModelState.AddModelError("Password", "Password and confirm password do not match");
                return View(registerVM);
            }
          
        }


    }
}
