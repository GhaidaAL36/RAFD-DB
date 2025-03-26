using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAFD.Models
{
    public class LoanOffer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int OfferID { get; set; }
        public int BankID { get; set; }
        public decimal InterestRate { get; set; }
        public decimal MaximumAmount { get; set; }
        public decimal MinimumSalary { get; set; }

        public Bank Bank { get; set; }  // Optional: Navigation
    }

}
