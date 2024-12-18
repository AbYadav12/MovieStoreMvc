using Microsoft.AspNetCore.Identity;

namespace MovieStoreMvc.Models.Domain
{
    public class ApplicationUsers :IdentityUser
    {
        public string Name { get; set; }
    }
}
