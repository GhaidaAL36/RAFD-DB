using Microsoft.AspNetCore.Mvc;
using RAFD.Models;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Globalization;



namespace RAFD.Controllers

{

    [Route("bank-dashboard")]

    public class BankDashboardController : Controller
    {
        private readonly AppDbContext _context;



        public BankDashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-bank-data")]
        public async Task<IActionResult> GetBankData()
        {
            var bankId = HttpContext.Session.GetInt32("BankID");
            Console.WriteLine("🔍 Session BankID = " + bankId);


            if (bankId == null)
                return Unauthorized();

            var bank = await _context.Banks.FindAsync(bankId);
            Console.WriteLine("🏦 Bank: " + bank?.BankName);

            var offers = await _context.LoanOffers
                .Where(o => o.BankID == bankId)
                .ToListAsync();
            Console.WriteLine("📦 Offers count: " + offers.Count);

            var offerIds = offers.Select(o => o.OfferID).ToList();

            var loanInfos = await _context.LoanInfos
                .Where(i => offerIds.Contains(i.OfferID))
                .ToListAsync();
            Console.WriteLine("📋 LoanInfos count: " + loanInfos.Count);

            var loanRequests = await _context.LoanRequests
                .Where(r => offerIds.Contains(r.OfferID))
                .ToListAsync();
            Console.WriteLine("📩 LoanRequests count: " + loanRequests.Count);

            return Json(new
            {
                bank = new
                {
                    bank.BankName,
                    bank.ContactInfo,
                    bank.Email
                },
                offers,
                loanInfos,
                loanRequests
            });
        }

        [HttpPost("add-offer")]
        public async Task<IActionResult> AddOffer([FromForm] string interestRate, [FromForm] string maxAmount, [FromForm] string minSalary)
        {
            try
            {
                var bankId = HttpContext.Session.GetInt32("BankID");
                if (bankId == null)
                    return Unauthorized();

                var offer = new LoanOffer
                {
                    BankID = bankId.Value,
                    InterestRate = decimal.Parse(interestRate, CultureInfo.InvariantCulture),
                    MaximumAmount = decimal.Parse(maxAmount, CultureInfo.InvariantCulture),
                    MinimumSalary = decimal.Parse(minSalary, CultureInfo.InvariantCulture)
                };

                _context.LoanOffers.Add(offer);
                await _context.SaveChangesAsync();

                return Ok(offer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error adding loan: " + ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("💥 Inner exception: " + ex.InnerException.Message);
                }

                return StatusCode(500, "Server error");
            }

        }

        [HttpPost("delete-offer")]
        public async Task<IActionResult> DeleteOffer(int offerId)
        {
            var offer = await _context.LoanOffers.FindAsync(offerId);
            if (offer == null)
                return NotFound();

            _context.LoanOffers.Remove(offer);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }

}