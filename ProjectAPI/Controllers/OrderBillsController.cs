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
    public class OrderBillsController : ControllerBase
    {
        private readonly Ass03Context _context;

        public OrderBillsController(Ass03Context context)
        {
            _context = context;
        }

        // GET: api/OrderBills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderBill>>> GetOrderBills()
        {
          if (_context.OrderBills == null)
          {
              return NotFound();
          }
            return await _context.OrderBills.ToListAsync();
        }

        // GET: api/OrderBills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderBill>> GetOrderBill(int id)
        {
          if (_context.OrderBills == null)
          {
              return NotFound();
          }
            var orderBill = await _context.OrderBills.FindAsync(id);

            if (orderBill == null)
            {
                return NotFound();
            }

            return orderBill;
        }

        // PUT: api/OrderBills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderBill(int id, OrderBill orderBill)
        {
            if (id != orderBill.OrderBillId)
            {
                return BadRequest();
            }

            _context.Entry(orderBill).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderBillExists(id))
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

        // POST: api/OrderBills
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OrderBill>> PostOrderBill(OrderBill orderBill)
        {
            int userId = orderBill.UserId;
            if (_context.OrderBills == null)
            {
                return Problem("Entity set 'Ass03Context.OrderBills' is null.");
            }

            // Add and save the order bill first
            _context.OrderBills.Add(orderBill);
            await _context.SaveChangesAsync();

            // Now, find the Cart associated with the UserId
            try
            {

            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart != null)
            {
                // Find all ProductCart entries associated with the CartId
                var productCarts = await _context.ProductCarts
                                      .Where(pc => pc.CartId == cart.CartId)
                                      .ToListAsync();

                // Remove all found ProductCart entries
                _context.ProductCarts.RemoveRange(productCarts);
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("============================================================");
            }
            }
            catch { }

            return CreatedAtAction("GetOrderBill", new { id = orderBill.OrderBillId }, orderBill);
        }



        // DELETE: api/OrderBills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderBill(int id)
        {
            if (_context.OrderBills == null)
            {
                return NotFound();
            }
            var orderBill = await _context.OrderBills.FindAsync(id);
            if (orderBill == null)
            {
                return NotFound();
            }

            _context.OrderBills.Remove(orderBill);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderBillExists(int id)
        {
            return (_context.OrderBills?.Any(e => e.OrderBillId == id)).GetValueOrDefault();
        }
    }
}
