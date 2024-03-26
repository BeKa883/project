using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ass03.Models;

namespace ProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCartsController : ControllerBase
    {
        private readonly Ass03Context _context;

        public ProductCartsController(Ass03Context context)
        {
            _context = context;
        }

        // GET: api/ProductCarts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCart>>> GetProductCarts()
        {
          if (_context.ProductCarts == null)
          {
              return NotFound();
          }
            return await _context.ProductCarts.ToListAsync();
        }

        // GET: api/ProductCarts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ProductCart>>> GetProductCart(int id)
        {
            if (_context.ProductCarts == null)
            {
                return NotFound();
            }

            var productCarts = await _context.ProductCarts
                                              .Where(pc => pc.CartId == id)
                                              .ToListAsync();

            if (productCarts == null || !productCarts.Any())
            {
                return NotFound();
            }

            return productCarts;
        }


        // PUT: api/ProductCarts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductCart(int id, ProductCart productCart)
        {
            if (id != productCart.ProductCartId)
            {
                return BadRequest();
            }

            _context.Entry(productCart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductCarts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductCart>> PostProductCart(ProductCart productCart)
        {
          if (_context.ProductCarts == null)
          {
              return Problem("Entity set 'Ass03Context.ProductCarts'  is null.");
          }
            _context.ProductCarts.Add(productCart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductCart", new { id = productCart.ProductCartId }, productCart);
        }

        // DELETE: api/ProductCarts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductCart(int id)
        {
            if (_context.ProductCarts == null)
            {
                return NotFound();
            }
            var productCart = await _context.ProductCarts.FindAsync(id);
            if (productCart == null)
            {
                return NotFound();
            }

            _context.ProductCarts.Remove(productCart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductCartExists(int id)
        {
            return (_context.ProductCarts?.Any(e => e.ProductCartId == id)).GetValueOrDefault();
        }
    }
}
