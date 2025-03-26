using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAFD.Models
{
    [Table("LoanInfo")]
    public class LoanInfo
    {
        [Key]
        public int DetailID { get; set; }

        public int OfferID { get; set; }
        public int Duration { get; set; }
        public decimal LoanAmount { get; set; }

        public LoanOffer Offer { get; set; }  // Optional: Navigation
    }


}
