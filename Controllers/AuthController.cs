using Microsoft.AspNetCore.Mvc;
using RAFD.Models;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace RAFD.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // ---------- STEP 1: Register Basic Info ----------
        [HttpPost]
        [Route("auth/register")]
        public async Task<IActionResult> Register(string userName, string email, string password)
        {
            if (await _context.Users.AnyAsync(u => u.UserEmail == email))
            {
                return BadRequest("Email is already registered");
            }

            var user = new User
            {
                UserName = userName,
                UserEmail = email,
                PasswordHash = HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Save User ID in Session (for Step 2)
            HttpContext.Session.SetInt32("UserID", user.UserID);

            return Redirect("/pages/additional-info.html");
        }

        // ---------- STEP 2: Save Additional Info ----------
        [HttpPost]
        [Route("auth/complete-registration")]
        public async Task<IActionResult> CompleteRegistration(int? userAge, string? userNationalID, string? userJopType, int? creditScore, string? userContactNum, decimal? userSalary)
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return BadRequest("Session expired or invalid user");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound();

            user.UserAge = userAge;
            user.UserNationalID = userNationalID;
            user.UserJopType = userJopType;
            user.CreditScore = creditScore;
            user.UserContactNum = userContactNum;
            user.UserSalary = userSalary;

            await _context.SaveChangesAsync();

            HttpContext.Session.Clear(); // Clear session after completion

            return Redirect("/pages/success.html");
        }

        // ---------- STEP 3: log in ----------
        [HttpPost]
        [Route("auth/login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Find user by email
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.UserEmail == email);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid email or password");
                return Redirect("/pages/login.html");
            }

            // ✅ Create session after successful login
            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("UserName", user.UserName);

            // ✅ Redirect to dashboard or home
            return Redirect("/pages/user-dashboard.html");
        }

        // display user info in dashboard
        [HttpGet]
        [Route("auth/get-user-info")]

        public async Task<IActionResult> GetUserInfo()
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
                return Unauthorized();

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            return Json(new
            {
                userName = user.UserName,
                userEmail = user.UserEmail,
                userAge = user.UserAge,
                userNationalID = user.UserNationalID,
                userJopType = user.UserJopType,
                userContactNum = user.UserContactNum,
                userSalary = user.UserSalary
            });
        }

        // check if user logged in 

        [HttpGet]
        [Route("auth/is-logged-in")]
        public IActionResult IsLoggedIn()
        {
            var userId = HttpContext.Session.GetInt32("UserID");

            if (userId != null)
            {
                return Json(new { isLoggedIn = true });
            }

            return Json(new { isLoggedIn = false });
        }

        // banks log in
        [HttpPost]
        [Route("auth/bank-login")]
        public async Task<IActionResult> BankLogin(string email, string password)
        {
            var bank = await _context.Banks.SingleOrDefaultAsync(b => b.Email == email);

            if (bank == null)
            {
                Console.WriteLine("❌ Bank not found");
                return Redirect("/pages/bank-login.html");
            }

            using (var sha256 = SHA256.Create())
            {
                var inputHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));

                Console.WriteLine("🔐 Entered password: " + password);
                Console.WriteLine("🔑 Hashed input:     " + inputHash);
                Console.WriteLine("🗄️ Stored hash:      " + bank.PasswordHash);

                if (inputHash != bank.PasswordHash)
                {
                    Console.WriteLine("❌ Password does not match");
                    return Redirect("/pages/bank-login.html");
                }
            }

            Console.WriteLine("✅ Bank logged in: " + bank.BankName);

            HttpContext.Session.SetInt32("BankID", bank.BankID);
            HttpContext.Session.SetString("BankName", bank.BankName);

            return Redirect("/pages/bank-interface.html");
        }


        // ---------- PASSWORD CHECK ----------
        private bool VerifyPassword(string password, string hash)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashedInput = Convert.ToBase64String(bytes);
                return hashedInput == hash;
            }
        }


        // ---------- PASSWORD HASHING ----------
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
