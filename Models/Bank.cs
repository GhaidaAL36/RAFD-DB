using System.ComponentModel.DataAnnotations;

namespace RAFD.Models
{
    public class Bank
    {
        [Key]
        public int BankID { get; set; }

        [Required]
        public string BankName { get; set; }

        [Required]
        public string ContactInfo { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
