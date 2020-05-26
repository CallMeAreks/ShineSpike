using System.ComponentModel.DataAnnotations;

namespace ShineSpike.ViewModels
{

    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; } = false;

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        }
    }
}
