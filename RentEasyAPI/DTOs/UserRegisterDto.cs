using System.ComponentModel.DataAnnotations;

namespace RentEasyAPI.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Length(8, 100, ErrorMessage = "Password must be between 8 and 100 characters")]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [AllowedValues("Landlord", "Tenant")]
        public string Role {  get; set; }
   
    }
}
