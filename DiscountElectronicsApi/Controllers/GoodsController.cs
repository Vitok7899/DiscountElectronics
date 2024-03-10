using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscountElectronicsApi.Models;
using DiscountElectronicsApi.Services;

namespace DiscountElectronicsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        private readonly discount_electronicContext _context;
        private readonly IGoodService goodService;

        public GoodsController(discount_electronicContext context, IGoodService goodService)
        {
            _context = context;
            this.goodService = goodService;
        }


        // GET: api/Goods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Good>>> GetGoods()
        {
            if (_context.Goods == null)
            {
                return NotFound();
            }
            return await _context.Goods.ToListAsync();
        }

        // GET: api/Goods/Category/5
        [HttpPost("Category/{id}")]
        public async Task<ActionResult<IEnumerable<Good>>> GetGood(int id, string? order, string? search, FilterModel? filter)
        {
            HashSet<Characteristic>? filterSet;
            if (filter == null)
            {
                filterSet = null;
            }
            else 
            {
                filterSet = filter.characteristics;
            }
            try 
            {
                var goods = goodService.GetFilteredGoodsByCategoryId(id, search, filterSet);
                return await Task.FromResult(goodService.GetSortedGoods(order, goods));
            }
            catch
            {
                return StatusCode(500);
            }
        }

        

        // PUT: api/Goods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGood(int? id, Good good)
        {
            if (id != good.IdGoods)
            {
                return BadRequest();
            }

            _context.Entry(good).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoodExists(id))
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

        // POST: api/Goods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Good>> PostGood(Good good)
        {
            if (_context.Goods == null)
            {
                return Problem("Entity set 'discount_electronicContext.Goods'  is null.");
            }
            _context.Goods.Add(good);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGood", new { id = good.IdGoods }, good);
        }

        // DELETE: api/Goods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGood(int? id)
        {
            if (_context.Goods == null)
            {
                return NotFound();
            }
            var good = await _context.Goods.FindAsync(id);
            if (good == null)
            {
                return NotFound();
            }

            _context.Goods.Remove(good);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GoodExists(int? id)
        {
            return (_context.Goods?.Any(e => e.IdGoods == id)).GetValueOrDefault();
        }
    }
}
