using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RAFD.Models;

namespace RAFD.Controllers
{
    [ApiController]
    [Route("api/loan-comparison")]
    public class LoanComparisonController : Controller
    {
        private readonly AppDbContext _context;

        public LoanComparisonController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("get-matches")]
        public async Task<IActionResult> GetMatchingOffers([FromBody] UserInputModel input)
        {
            var matches = await _context.LoanInfos
                .Include(info => info.Offer)
                .ThenInclude(offer => offer.Bank) // to access BankName
                .Where(info =>
                    info.LoanAmount >= input.Amount &&
                    info.Duration == input.Duration &&
                    info.Offer.MinimumSalary <= input.Salary)
                .OrderBy(info => info.Offer.InterestRate)
                .ToListAsync();

            if (!matches.Any())
                return NotFound("لا توجد عروض مناسبة.");

            var result = matches.Select(item =>
            {
                var P = item.LoanAmount;
                var r = item.Offer.InterestRate / 12 / 100; // Monthly interest rate
                var n = item.Duration;

                decimal monthlyPayment;
                if (r == 0)
                {
                    monthlyPayment = P / n;
                }
                else
                {
                    monthlyPayment = P * r * (decimal)Math.Pow(1 + (double)r, n) /
                                     (decimal)(Math.Pow(1 + (double)r, n) - 1);
                }

                return new
                {
                    bankName = item.Offer.Bank.BankName,
                    loanAmount = P,
                    duration = n,
                    interestRate = item.Offer.InterestRate,
                    monthlyPayment = Math.Round(monthlyPayment, 2)
                };
            });

            return Ok(result);
        }

    }
}

public class UserInputModel
{
    public decimal Salary { get; set; }
    public decimal Amount { get; set; }
    public int Duration { get; set; }
}


