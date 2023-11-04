using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [EmailAddress]
        public string DisplayName { get; set; }

        [Required]
        [RegularExpression(
            "(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
            ErrorMessage ="Password must have 1 Uppercase Letter, 1 Lowercase Letter, 1 Non-Alphanumeric character, And be at least have 6 Characters"         
            )            
        ]
        public string Password { get; set; }
    }
}