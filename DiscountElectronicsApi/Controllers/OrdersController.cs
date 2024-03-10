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
using System.Drawing.Text;
using DiscountElectronicsApi.Services;

namespace DiscountElectronicsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly discount_electronicContext _context;
        private IGoodService goodService;
        private IShoppingCartService shopCartService;

        public OrdersController(discount_electronicContext context, IGoodService goodService, IShoppingCartService shopCartService)
        {
            _context = context;
            this.goodService = goodService;
            this.shopCartService = shopCartService;
        }

        public static int GetTokenInfo(string token)
        {
            var t = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return Convert.ToInt32(t.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
        }

        public static string GenUniqueOrderNum(string prefix, string suffix, int length)
        {
            string id = Guid.NewGuid().ToString().Substring(0, length);

            string name = string.Format("{0}-{1}{2}", prefix, id, suffix);

            return name;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int? id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpGet("content/{orderid}")]
        public async Task<ActionResult<ShoppingCartDTO>> GetOrderContent([FromHeader] FromHeaderModel headers, int orderid)
        {
            if (_context.ShoppingCarts == null)
            {
                return NotFound();
            }

            var userid = GetTokenInfo(headers.Authorization);

            var shoppingCart = _context.ShoppingCarts.Where(x => x.IdUser == userid && x.IdOrder == orderid).ToList();

            var shoppingCartDTO = new ShoppingCartDTO
            {
                GoodCounts = new Dictionary<int, int>(),
                Goods = new List<Good>(),
                TotalCost = 0
            };

            foreach (ShoppingCart cart in shoppingCart)
            {
                shoppingCartDTO.GoodCounts[cart.IdGoods] = cart.Count;
                var good = _context.Goods.Where(g => g.IdGoods == cart.IdGoods).FirstOrDefault();
                shoppingCartDTO.Goods.Add(good);
                shoppingCartDTO.TotalCost += goodService.CalcCartGoodSum(cart.IdGoods, cart.Count);
            }
            return shoppingCartDTO;
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders([FromHeader] FromHeaderModel headers)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            var userid = GetTokenInfo(headers.Authorization);
            
            return await _context.Orders.Where(o => o.IdUser == userid).ToListAsync();
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int? id, Order order)
        {
            if (id != order.IdOrder)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{idbranch}")]
        public async Task<ActionResult<Order>> PostOrder([FromHeader] FromHeaderModel headers, int idbranch)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'discount_electronicContext.Orders'  is null.");
            }

            var idUser = GetTokenInfo(headers.Authorization);
            var branch = _context.Branches.Where(b => b.IdBranches == idbranch).FirstOrDefault();
            if (branch == null)
            {
                return BadRequest("Некорректный адрес пункта выдачи");
            }

            string genOrderNum;

            while (true)
            {
                genOrderNum = GenUniqueOrderNum("АА", "1", 3);
                var orders = _context.Orders.Where(o => o.OrderNumber.Equals(genOrderNum)).FirstOrDefault();
                if (orders == null)
                {
                    break;
                }
            }

            var shoppingCartDTO = shopCartService.GetShoppingCart(idUser);
            var total = shoppingCartDTO.TotalCost;

            var order = new Order
            {
                OrderNumber = genOrderNum,
                IdBranches = idbranch,
                IdStatus = 2,
                IdUser = idUser,
                OrderCost = total
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            var shoppingCart = _context.ShoppingCarts.Where(s => s.IdUser == idUser && s.IdOrder == null).ToList();
            foreach(var cart in shoppingCart)
            {
                cart.IdOrder = order.IdOrder;
            }
            _context.SaveChanges();
            return CreatedAtAction("GetOrder", new { id = order.IdOrder }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int? id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int? id)
        {
            return (_context.Orders?.Any(e => e.IdOrder == id)).GetValueOrDefault();
        }
    }
}
