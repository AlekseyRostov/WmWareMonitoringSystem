using System.ComponentModel.DataAnnotations;

namespace OwinWebApiSelfHost.Model
{
    public class VmAccountBindingModels
    {
        [Required]
        [Display(Name = "Email")]
        public string Url { get; set; }

        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}
