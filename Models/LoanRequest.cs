using System.ComponentModel.DataAnnotations;

namespace RAFD.Models
{
    public class LoanRequest
    {
        [Key]
        public int RequestID { get; set; }
        public int UserID { get; set; }
        public int OfferID { get; set; }
        public string Status { get; set; }
        public DateTime SubmissionDate { get; set; }

        public LoanOffer Offer { get; set; }  // Optional
    }


}
