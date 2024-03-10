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
    [Route("api/[controller]")]
    [ApiController]
    public class AllCharacteristicsController : ControllerBase
    {
        private readonly discount_electronicContext _context;

        public AllCharacteristicsController(discount_electronicContext context)
        {
            _context = context;
        }

        // GET: api/AllCharacteristics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllCharacteristic>>> GetAllCharacteristics()
        {
            if (_context.AllCharacteristics == null)
            {
                return NotFound();
            }
            return await _context.AllCharacteristics.ToListAsync();
        }

        // GET: api/AllCharacteristics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AllCharacteristic>> GetAllCharacteristic(int? id)
        {
            if (_context.AllCharacteristics == null)
            {
                return NotFound();
            }
            var allCharacteristic = await _context.AllCharacteristics.FindAsync(id);

            if (allCharacteristic == null)
            {
                return NotFound();
            }

            return allCharacteristic;
        }

        // PUT: api/AllCharacteristics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAllCharacteristic(int? id, AllCharacteristic allCharacteristic)
        {
            if (id != allCharacteristic.IdAllCharacteristic)
            {
                return BadRequest();
            }

            _context.Entry(allCharacteristic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AllCharacteristicExists(id))
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

        // POST: api/AllCharacteristics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AllCharacteristic>> PostAllCharacteristic(AllCharacteristic allCharacteristic)
        {
            if (_context.AllCharacteristics == null)
            {
                return Problem("Entity set 'discount_electronicContext.AllCharacteristics'  is null.");
            }
            _context.AllCharacteristics.Add(allCharacteristic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAllCharacteristic", new { id = allCharacteristic.IdAllCharacteristic }, allCharacteristic);
        }

        // DELETE: api/AllCharacteristics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllCharacteristic(int? id)
        {
            if (_context.AllCharacteristics == null)
            {
                return NotFound();
            }
            var allCharacteristic = await _context.AllCharacteristics.FindAsync(id);
            if (allCharacteristic == null)
            {
                return NotFound();
            }

            _context.AllCharacteristics.Remove(allCharacteristic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AllCharacteristicExists(int? id)
        {
            return (_context.AllCharacteristics?.Any(e => e.IdAllCharacteristic == id)).GetValueOrDefault();
        }
    }
}
