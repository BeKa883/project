using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ass03.Models;

namespace Ass03.Controllers.adminContrllers
{
    public class ProductsController : Controller
    {
        private readonly Ass03Context _context;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(Ass03Context context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString)
        {
            var products = _context.Products.Include(p => p.Category).Include(p => p.User);
            if (products == null)
            {
                return Problem("Entity set 'Ass03Context.Products'  is null.");
            }
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString)).Include(p => p.Category).Include(p => p.User);
            }
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (product.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Image is required.");
            }


            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
                ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username");

                return View();


            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(product.ImageFile!.FileName);

            string imagePath = _environment.WebRootPath + "/products/" + newFileName;
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await product.ImageFile.CopyToAsync(fileStream);
                product.ImageUrl = newFileName;
                product.Date = DateTime.Now;
                Product pro = new Product()
                {
                    ProductName = product.ProductName,
                    UnitStock = product.UnitStock,
                    UnitPrice = product.UnitPrice,
                    ImageUrl = product.ImageUrl,
                    Date = DateTime.Now,
                    CategoryId = product.CategoryId,
                    Status = product.Status,
                    UserId = product.UserId,


                };
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }



            return RedirectToAction(nameof(Index), "Products");
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username", product.UserId);
            ViewData["imgURLName"] = product.ImageUrl;
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            var pro = _context.Products.AsNoTracking().ToList().Where(p => p.ProductId == id).FirstOrDefault();
            if (pro == null)
            {
                return RedirectToAction(nameof(Index));
            }
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (product.ImageFile == null)
                    {
                        product.ImageUrl = pro.ImageUrl;
                        product.Date = DateTime.Now;
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }

                    string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    newFileName += Path.GetExtension(product.ImageFile!.FileName);

                    string imagePath = _environment.WebRootPath + "/products/" + newFileName;
                    using (var fileStream = new FileStream(imagePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                        product.ImageUrl = newFileName;
                        product.Date = DateTime.Now;
                    }

                    //delete old image
                    string oldImagePath = _environment.WebRootPath + "/products/" + pro.ImageUrl;
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    product.ImageUrl = newFileName;
                    product.Date = DateTime.Now;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Username", product.UserId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'Ass03Context.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
