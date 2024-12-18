using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Models.DTO;
using MovieStoreMvc.Respositories.Abstract;
using System.Security.Claims;

namespace MovieStoreMvc.Respositories.Implementation
{
    public class UserAuthenticationService :IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUsers> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUsers> signInManager;

        public UserAuthenticationService(UserManager<ApplicationUsers> userManager,
           SignInManager<ApplicationUsers> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }
        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if(userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "User Already Exists";
                return status;
            }
            ApplicationUsers user = new ApplicationUsers()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Name = model.Name,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if(!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User creation failed";
                return status;
            }
            if(!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));

            if(await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user,model.Role);
            }
            status.StatusCode = 1;
            status.Message = "You Have Registered SuccesFully";
            return status;
        }

        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status = new Status();
            var user = await userManager.FindByNameAsync(model.Username);
            if(user == null)
            {
                status.StatusCode = 0;
                status.Message = "Invalid Username";
                return status;
            }
            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid Password";
                return status;
            }
            var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, true, true);
            if(signInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                status.StatusCode = 1;
                status.Message = "Loggeg In SuccessFully";
            }
            else if(signInResult.IsLockedOut)
            {
                status.StatusCode= 0;
                status.Message = "User is Locked Out";

            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error on Logging in";
            }
            return status;
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        //public Task<Status> RegisterAsync(RegistrationModel model)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
