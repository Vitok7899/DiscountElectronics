using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscountElectronicsApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DiscountElectronicsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly discount_electronicContext _context;

        public ProfilesController(discount_electronicContext context)
        {
            _context = context;
        }
        public static int GetTokenInfo(string token)
        {
            var t = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return Convert.ToInt32(t.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profile>>> GetProfiles()
        {
            if (_context.Profiles == null)
            {
                return NotFound();
            }
            return await _context.Profiles.ToListAsync();
        }

        // GET: api/Profiles/5
        [HttpGet("user")]
        public async Task<ActionResult<Profile>> GetProfile([FromHeader] FromHeaderModel headers)
        {
          if (_context.Profiles == null)
          {
              return NotFound();
          }

            var profileid = GetTokenInfo(headers.Authorization);
            var profile = await _context.Profiles.Where(p => p.IdUsers == profileid).FirstOrDefaultAsync();

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }

        // PUT: api/Profiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfile(int? id, Profile profile)
        {
            if (id != profile.IdUsers)
            {
                return BadRequest();
            }

            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
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

        // POST: api/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Profile>> PostProfile(Profile profile)
        {
          if (_context.Profiles == null)
          {
              return Problem("Entity set 'discount_electronicContext.Profiles'  is null.");
          }
            _context.Profiles.Add(profile);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProfileExists(profile.IdUsers))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProfile", new { id = profile.IdUsers }, profile);
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int? id)
        {
            if (_context.Profiles == null)
            {
                return NotFound();
            }
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfileExists(int? id)
        {
            return (_context.Profiles?.Any(e => e.IdUsers == id)).GetValueOrDefault();
        }
    }
}
