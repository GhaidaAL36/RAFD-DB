using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAFD.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        public int? UserAge { get; set; }

        [MaxLength(15)]
        public string? UserNationalID { get; set; }

        [MaxLength(50)]
        public string? UserJopType { get; set; }

        public int? CreditScore { get; set; }

        [MaxLength(15)]
        public string? UserContactNum { get; set; }

        public decimal? UserSalary { get; set; }

        public byte[]? FingerPrintData { get; set; }

        [Required]
        public string PasswordHash { get; set; }
    }
}
