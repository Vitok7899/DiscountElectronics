using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscountElectronicsApi.Models;

namespace DiscountElectronicsApi.Controllers
{
    [Route("api/characteristic")]
    [ApiController]
    public class CharacteristicController : ControllerBase
    {
        private readonly discount_electronicContext _context;

        public CharacteristicController(discount_electronicContext context)
        {
            _context = context;
        }

        //// GET: api/AllGoods
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<AllGood>>> GetAllGoods()
        //{
        //  if (_context.AllGoods == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.AllGoods.ToListAsync();
        //}

        // GET: api/AllGoods/5
        [HttpGet("good/{id}")]
        public async Task<ActionResult<IEnumerable<AllGood>>> GetAllGood(int id)
        {
          if (_context.AllGoods == null)
          {
              return NotFound();
          }
            var allGood = await _context.AllGoods.Where(c => c.IdGoods == id).ToListAsync();

            if (allGood == null)
            {
                return NotFound();
            }

            return allGood;
        }

        [HttpGet("category/{id}")]
        public async Task<ActionResult<IEnumerable<Characteristic>>> GetCategoryCharacteristic(int id)
        {
            if (_context.AllGoods == null)
            {
                return NotFound();
            }
            var categoryAllGoods = await _context.AllGoods.Where(c => c.IdCategory == id).ToListAsync();

            if (categoryAllGoods == null)
            {
                return NotFound();
            }
            HashSet<Characteristic> categoryCharacteristics = new HashSet<Characteristic>();
            foreach (var categoryAllGood in categoryAllGoods) {
                var characteristic = await _context.Characteristics
                    .Where(c => c.IdCharacteristics == categoryAllGood.IdCharacteristics).ToListAsync();
                if (characteristic != null)
                {
                    categoryCharacteristics.Add(characteristic[0]);
                }
            }
            return categoryCharacteristics;
        }



        //// PUT: api/AllGoods/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAllGood(int id, AllGood allGood)
        //{
        //    if (id != allGood.IdGoods)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(allGood).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AllGoodExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/AllGoods
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<AllGood>> PostAllGood(AllGood allGood)
        //{
        //  if (_context.AllGoods == null)
        //  {
        //      return Problem("Entity set 'discount_electronicContext.AllGoods'  is null.");
        //  }
        //    _context.AllGoods.Add(allGood);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (AllGoodExists(allGood.IdGoods))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetAllGood", new { id = allGood.IdGoods }, allGood);
        //}

        //// DELETE: api/AllGoods/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteAllGood(int id)
        //{
        //    if (_context.AllGoods == null)
        //    {
        //        return NotFound();
        //    }
        //    var allGood = await _context.AllGoods.FindAsync(id);
        //    if (allGood == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.AllGoods.Remove(allGood);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool AllGoodExists(int id)
        //{
        //    return (_context.AllGoods?.Any(e => e.IdGoods == id)).GetValueOrDefault();
        //}
    }
}
