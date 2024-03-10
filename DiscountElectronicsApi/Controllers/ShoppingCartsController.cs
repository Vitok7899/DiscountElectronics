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
using DiscountElectronicsApi.Services;

namespace DiscountElectronicsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly discount_electronicContext _context;
        private IGoodService goodService;
        private IShoppingCartService shopCartService;

        public ShoppingCartsController(discount_electronicContext context, IGoodService service, IShoppingCartService shopCartService)
        {
            _context = context;
            goodService = service;
            this.shopCartService = shopCartService;
        }

        // GET: api/ShoppingCarts
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetShoppingCarts()
        //{
        //  if (_context.ShoppingCarts == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.ShoppingCarts.ToListAsync();
        //}

        public static int GetTokenInfo(string token)
        {
            var t = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return Convert.ToInt32(t.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
        }

        // GET: api/ShoppingCarts/5
        [HttpGet]
        public async Task<ActionResult<ShoppingCartDTO>> GetShoppingCart([FromHeader]FromHeaderModel headers)
        {
          if (_context.ShoppingCarts == null)
          {
              return NotFound();
          }

            var userId = GetTokenInfo(headers.Authorization);
           
            return shopCartService.GetShoppingCart(userId);
        }

        // PUT: api/ShoppingCarts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{goodid}")]
        public async Task<ActionResult<ShoppingCart>> PutShoppingCart([FromHeader] FromHeaderModel headers, int goodid, [FromQuery]int count)
        {
            if (_context.ShoppingCarts == null)
            {
                return Problem("Entity set 'discount_electronicContext.ShoppingCarts'  is null.");
            }
            var idUser = GetTokenInfo(headers.Authorization);
            var shoppingCart = _context.ShoppingCarts.Where(s => s.IdUser == idUser && s.IdGoods == goodid && s.IdOrder == null).FirstOrDefault();

            if (shoppingCart == null || count < 0)
            {
                return BadRequest();
            }

            if (count == 0)
            {
                await DeleteShoppingCart(headers, goodid);
                return NoContent();
            }

            if (shoppingCart.Count != count)
            { 
                shoppingCart.Count = count;
                await _context.SaveChangesAsync();
            }
            return CreatedAtAction("GetShoppingCart", new { id = shoppingCart.IdShoppingCart }, shoppingCart);
        }

        // POST: api/ShoppingCarts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> PostShoppingCart([FromHeader]FromHeaderModel headers, int goodid, int count)
        {
          if (_context.ShoppingCarts == null)
          {
              return Problem("Entity set 'discount_electronicContext.ShoppingCarts'  is null.");
          }

            var idUser = GetTokenInfo(headers.Authorization);
            var shoppingCartAllready = _context.ShoppingCarts.Where(s => s.IdUser == idUser && s.IdGoods == goodid && s.IdOrder == null).FirstOrDefault();

            if ((!goodService.CheckGoodExist(goodid)) || shoppingCartAllready != null || count < 0)
            {
                return BadRequest();
            }

            var shoppingCart = new ShoppingCart
            {
                Count = count,
                IdGoods = goodid,
                IdUser = idUser
            }; 

            _context.ShoppingCarts.Add(shoppingCart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingCart", new { id = shoppingCart.IdShoppingCart }, shoppingCart);
        }

        // DELETE: api/ShoppingCarts/5
        [HttpDelete("{goodid}")]
        public async Task<IActionResult> DeleteShoppingCart([FromHeader] FromHeaderModel headers, int goodid)
        {
            if (_context.ShoppingCarts == null)
            {
                return NotFound();
            }
            var idUser = GetTokenInfo(headers.Authorization);
            var shoppingCart = _context.ShoppingCarts.Where(s => s.IdUser == idUser && s.IdGoods == goodid).FirstOrDefault();
            if (shoppingCart == null)
            {
                return NotFound();
            }

            _context.ShoppingCarts.Remove(shoppingCart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAllShoppingCart([FromHeader] FromHeaderModel headers)
        {
            if (_context.ShoppingCarts == null)
            {
                return NotFound();
            }
            var idUser = GetTokenInfo(headers.Authorization);
            var shoppingCart = _context.ShoppingCarts.Where(s => s.IdUser == idUser && s.IdOrder == null).ToList();
            if (shoppingCart == null || shoppingCart.Count==0)
            {
                return NotFound();
            }

            foreach(ShoppingCart cart in shoppingCart)
            {
                _context.ShoppingCarts.Remove(cart);              
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ShoppingCartExists(int? id)
        {
            return (_context.ShoppingCarts?.Any(e => e.IdShoppingCart == id)).GetValueOrDefault();
        }
    }
}
