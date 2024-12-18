using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Models.DTO;

namespace MovieStoreMvc.Respositories.Abstract
{
    public interface IMovieService
    {
        bool Add(Movie model);
        bool Update(Movie model);   
        Movie GetById(int id);      
        bool Delete(int id);
        MovieListVm List(string term ="", bool paging = false, int currentPage = 0);
        List<int> GetGenreByMovieId(int movieId);
    }
}
