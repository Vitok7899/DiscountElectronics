using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiscountElectronicsApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DiscountElectronicsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly discount_electronicContext _context;

        public static int iduser;

        [HttpPost("RefreshToken")]
        public async Task<ActionResult> RefreshToken(string tokenold)
        {
            var userId = GetTokenInfo(tokenold);
            User user = _context.Users.FirstOrDefault(x => x.IdUsers == userId);
            var token = generateToken(user);

            return Ok(token);
        }

        private string generateSalt()
        {
            byte[] bytes = new byte[32];
            Random.Shared.NextBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
        private string generateHashPassword(string password, string salt)
        {

            var passwordSalt = $"{password}{salt}";

            var passwordByte = Encoding.UTF8.GetBytes(passwordSalt);

            var sha = new SHA256Managed();

            var hashPasswordByte = sha.ComputeHash(passwordByte);

            return Convert.ToBase64String(hashPasswordByte);
        }

        private bool isCheckPassword(string salt, string oldPassword, string newPassword)
        {
            string hasNewPassword = generateHashPassword(newPassword, salt);
            return oldPassword == hasNewPassword;
        }

        //private List<User> user = _context.Users.ToList();

        public class Test
        {
            public string username { get; set; }

            public string password { get; set; }
        }

        [HttpPost("Sign_in")]
        public async Task<ActionResult<string>> signIn([FromBody] Test test)
        {

            var resultUser = _context.Users.FirstOrDefault(x => x.Email == test.username);

            if (resultUser == null)
            {
                return BadRequest("Такого пользователя не существует");
            }


            var saltpassword = generateHashPassword(test.password, resultUser.Salt);

            if (!isCheckPassword(resultUser.Salt, resultUser.Password, test.password))
            {
                return BadRequest("Пароль не совпадает");
            }


            var token = generateToken(resultUser);

            return Ok(token);
        }

        [HttpPost("sign_up")]
        public async Task<ActionResult<string>> signUp(
            AuthDto dto)
        {
            HashAlgorithm hash = new SHA256Managed();

            string saltGenerate = generateSalt();

            string hashPassword = generateHashPassword(dto.Password, saltGenerate);

            User user = new User();
            Profile profile = new Profile();

            user.IdUsers = null;
            user.Email = dto.Email;
            user.Password = hashPassword.ToString();
            user.Salt = saltGenerate;
            user.IdRole = 1;

            _context.Add(user);
            await _context.SaveChangesAsync();

            profile.IdUsers = user.IdUsers;
            profile.NumPhone = dto.NumPhone;
            profile.Surname = dto.Surname;
            profile.Name = dto.Name;
            profile.SecondName = dto.SecondName;
            profile.Email = dto.Email;

            _context.Add(profile);
            await _context.SaveChangesAsync();
            Test test = new Test
            {
                username = dto.Email,
                password= dto.Password
            };
            return await signIn(test);
        }

        //[HttpPost("sign_up_emp")]
        //public async Task<ActionResult<AuthDto>> signUpEmp(
        //    EmployeeDto dtos)
        //{
        //    HashAlgorithm hash = new SHA256Managed();

        //    string saltGenerate = generateSalt();

        //    string hashPassword = generateHashPassword(dtos.password, saltGenerate);

        //    User user = new User();


        //    user.IdUsers = null;
        //    user.UserMail = dtos.email;
        //    user.UserPassword = hashPassword.ToString();
        //    user.SecondName = dtos.SecondName;
        //    user.Name = dtos.Name;
        //    user.Surname = dtos.Surname;
        //    user.Salt = saltGenerate;
        //    user.IdRole = 2;

        //    _context.Add(user);
        //    await _context.SaveChangesAsync();
        //    return Ok(dtos);
        //}

        public static string? generateToken(User user)
        {
            var identity = new ClaimsIdentity();

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.IdUsers.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType,user.IdRole.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                identity = claimsIdentity;

                var jwt = new JwtSecurityToken(
                        issuer: AuthOpt.ISSUER,
                        audience: AuthOpt.AUDIENCE,
                notBefore: DateTime.UtcNow,
                claims: identity.Claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOpt.LIFETIME)),
                        signingCredentials: new SigningCredentials(AuthOpt.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }

            // если пользователя не найдено
            return null;
        }

        public static int GetTokenInfo(string token)
        {
            var t = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return Convert.ToInt32(t.Claims.FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
        }

        public UsersController(discount_electronicContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int? id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int? id, User user)
        {
            if (id != user.IdUsers)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'discount_electronicContext.Users'  is null.");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.IdUsers }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int? id)
        {
            return (_context.Users?.Any(e => e.IdUsers == id)).GetValueOrDefault();
        }
    }
}
