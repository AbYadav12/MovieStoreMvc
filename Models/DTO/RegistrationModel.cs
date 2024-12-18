using System.ComponentModel.DataAnnotations;

namespace MovieStoreMvc.Models.DTO
{
    public class RegistrationModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email {  get; set; }
        public string UserName {  get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirm {  get; set; }
        public string Role {  get; set; }

    }
}
