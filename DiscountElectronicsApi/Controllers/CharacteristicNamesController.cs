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
    public class CharacteristicNamesController : ControllerBase
    {
        private readonly discount_electronicContext _context;

        public CharacteristicNamesController(discount_electronicContext context)
        {
            _context = context;
        }

        // GET: api/CharacteristicNames
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacteristicName>>> GetCharacteristicNames()
        {
            if (_context.CharacteristicNames == null)
            {
                return NotFound();
            }
            return await _context.CharacteristicNames.ToListAsync();
        }

        // GET: api/CharacteristicNames/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacteristicName>> GetCharacteristicName(int? id)
        {
            if (_context.CharacteristicNames == null)
            {
                return NotFound();
            }
            var characteristicName = await _context.CharacteristicNames.FindAsync(id);

            if (characteristicName == null)
            {
                return NotFound();
            }

            return characteristicName;
        }

        // PUT: api/CharacteristicNames/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacteristicName(int? id, CharacteristicName characteristicName)
        {
            if (id != characteristicName.IdCharacteristicName)
            {
                return BadRequest();
            }

            _context.Entry(characteristicName).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CharacteristicNameExists(id))
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

        // POST: api/CharacteristicNames
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CharacteristicName>> PostCharacteristicName(CharacteristicName characteristicName)
        {
            if (_context.CharacteristicNames == null)
            {
                return Problem("Entity set 'discount_electronicContext.CharacteristicNames'  is null.");
            }
            _context.CharacteristicNames.Add(characteristicName);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCharacteristicName", new { id = characteristicName.IdCharacteristicName }, characteristicName);
        }

        // DELETE: api/CharacteristicNames/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacteristicName(int? id)
        {
            if (_context.CharacteristicNames == null)
            {
                return NotFound();
            }
            var characteristicName = await _context.CharacteristicNames.FindAsync(id);
            if (characteristicName == null)
            {
                return NotFound();
            }

            _context.CharacteristicNames.Remove(characteristicName);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CharacteristicNameExists(int? id)
        {
            return (_context.CharacteristicNames?.Any(e => e.IdCharacteristicName == id)).GetValueOrDefault();
        }
    }
}
