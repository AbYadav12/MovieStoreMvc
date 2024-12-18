using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MovieStoreMvc.Models.DTO;

namespace MovieStoreMvc.Respositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel login);

        Task LogoutAsync();
        Task<Status> RegisterAsync(RegistrationModel model);
        //Task<Status> ChangePasswordAsync(ChangePasswordModel model,string username)
    }
}
