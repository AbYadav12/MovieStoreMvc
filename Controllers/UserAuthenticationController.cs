using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MovieStoreMvc.Models.DTO;
using MovieStoreMvc.Respositories.Abstract;

namespace MovieStoreMvc.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private IUserAuthenticationService authService;
        public UserAuthenticationController(IUserAuthenticationService authService)
        {
                this.authService = authService;
        }
       /* public async Task<IActionResult> Register()
        {
            // we will create a user with admin right after that we are going to comments this method and because we need only 
            // one user in this applications
            //And If you need other users,you can implement this registration method with view 
            var model = new RegistrationModel
            {
                Email = "admin@gmail.com",
                UserName = "admin",
                Name = "Abineet",
                Password = "Admin@123",
                PasswordConfirm = "Admin@123",
                Role = "Admin",
            };
            // If you want to register with user change Role ="user"
             var result =await authService.RegisterAsync(model);
            return Ok(result.Message);
        }*/
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await authService.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["msg"] = "Could Not logged In";
                return RedirectToAction(nameof(Login));
            }
        }
        public async Task<IActionResult> Logout()
        {
           await authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

    }
}
