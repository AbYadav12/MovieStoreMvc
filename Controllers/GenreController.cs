using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Respositories.Abstract;

namespace MovieStoreMvc.Controllers
{
    [Authorize]
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Genre model)
        {
            if (!ModelState.IsValid)
            {
                // Return view with validation errors
                return View(model);
            }

            var result = _genreService.Add(model);
            if (result)
            {
                TempData["msg"] = "Successfully added.";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "An error occurred while adding the genre.";
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var data = _genreService.GetById(id);
            if (data == null)
            {
                TempData["msg"] = "Genre not found.";
                return RedirectToAction(nameof(GenreList));
            }

            return View(data);
        }

        [HttpPost]
        public IActionResult Update(Genre model)
        {
            if (!ModelState.IsValid)
            {
                // Return view with validation errors
                return View(model);
            }

            var result = _genreService.Update(model);
            if (result)
            {
                TempData["msg"] = "Successfully updated.";
                return RedirectToAction(nameof(GenreList));
            }
            else
            {
                TempData["msg"] = "An error occurred while updating the genre.";
                return View(model);
            }
        }

        public IActionResult GenreList()
        {
            var data = _genreService.List().ToList();
            return View(data);
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["msg"] = "Invalid Genre ID.";
                return RedirectToAction(nameof(GenreList));
            }

            try
            {
                // Check if the genre exists before attempting deletion
                var genre = _genreService.GetById(id);
                if (genre == null)
                {
                    TempData["msg"] = "Genre not found. Unable to delete.";
                    return RedirectToAction(nameof(GenreList));
                }

                // Attempt to delete
                var isDeleted = _genreService.Delete(id);
                TempData["msg"] = isDeleted
                    ? "Genre successfully deleted."
                    : "Failed to delete the genre due to an unknown error.";
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                // logger.LogError(ex, "Error occurred while deleting genre.");

                TempData["msg"] = $"An unexpected error occurred: {ex.Message}";
            }

            return RedirectToAction(nameof(GenreList));
        }



    }
}
