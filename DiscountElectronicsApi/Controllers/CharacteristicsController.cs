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
    public class CharacteristicsController : ControllerBase
    {
        private readonly discount_electronicContext _context;

        public CharacteristicsController(discount_electronicContext context)
        {
            _context = context;
        }

        // GET: api/Characteristics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Characteristic>>> GetCharacteristics()
        {
            if (_context.Characteristics == null)
            {
                return NotFound();
            }
            return await _context.Characteristics.ToListAsync();
        }

        // GET: api/Characteristics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Characteristic>> GetCharacteristic(int? id)
        {
            if (_context.Characteristics == null)
            {
                return NotFound();
            }
            var characteristic = await _context.Characteristics.FindAsync(id);

            if (characteristic == null)
            {
                return NotFound();
            }

            return characteristic;
        }

        // PUT: api/Characteristics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacteristic(int? id, Models.Characteristic characteristic)
        {
            if (id != characteristic.IdCharacteristics)
            {
                return BadRequest();
            }

            _context.Entry(characteristic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacteristicExists(id))
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

        // POST: api/Characteristics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Characteristic>> PostCharacteristic(Models.Characteristic characteristic)
        {
            if (_context.Characteristics == null)
            {
                return Problem("Entity set 'discount_electronicContext.Characteristics'  is null.");
            }
            _context.Characteristics.Add(characteristic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCharacteristic", new { id = characteristic.IdCharacteristics }, characteristic);
        }

        // DELETE: api/Characteristics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacteristic(int? id)
        {
            if (_context.Characteristics == null)
            {
                return NotFound();
            }
            var characteristic = await _context.Characteristics.FindAsync(id);
            if (characteristic == null)
            {
                return NotFound();
            }

            _context.Characteristics.Remove(characteristic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CharacteristicExists(int? id)
        {
            return (_context.Characteristics?.Any(e => e.IdCharacteristics == id)).GetValueOrDefault();
        }
    }
}
