using Microsoft.EntityFrameworkCore;

namespace RAFD.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } // for users table
        public DbSet<Bank> Banks { get; set; } // for banks table
        public DbSet<LoanOffer> LoanOffers { get; set; } //LoanOffers table
        public DbSet<LoanInfo> LoanInfos { get; set; }// LoanInfos table
        public DbSet<LoanRequest> LoanRequests { get; set; } // LoanRequests 


    }
}
