using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Respositories.Abstract;

namespace MovieStoreMvc.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IFileService _fileService;
        private readonly IGenreService _genService;

        public MovieController(IMovieService movieService, IFileService fileService, IGenreService genService)
        {
            _movieService = movieService;
            _fileService = fileService;
            _genService = genService;

        }

        public IActionResult Add()
        {
            var model = new Movie();
            model.GenreList = _genService.List().Select(a => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = a.GenreName, Value = a.ID.ToString() });
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Movie model)
        {
            model.GenreList = _genService.List().Select(a => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem { Text = a.GenreName, Value = a.ID.ToString()});
            //if (!ModelState.IsValid)
            //{
            //    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //    {
            //        Console.WriteLine(error.ErrorMessage);
            //    }
            //    return View(model);
            //}
            if (model.ImageFile != null)
            {
                var fileReult = this._fileService.SaveImage(model.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileReult.Item2;
                model.MovieImage = imageName;
            }
            var result = _movieService.Add(model);

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
            var model = _movieService.GetById(id);

            if (model == null)
            {
                TempData["msg"] = "Movie not found.";
                return RedirectToAction(nameof(MovieList));
            }

            // Get the list of all genres and mark those associated with the movie as selected
            var selectedGenres = _movieService.GetGenreByMovieId(id); // Replace with your method to get genres for a movie
            model.GenreList = _genService.List().Select(genre => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = genre.GenreName,
                Value = genre.ID.ToString(),
                Selected = selectedGenres.Contains(genre.ID)
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Movie model)
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                // Re-populate GenreList for the view
                model.GenreList = _genService.List().Select(a => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = a.GenreName,
                    Value = a.ID.ToString(),
                    Selected = model.Genres.Contains(int.Parse(a.ID.ToString())) // Ensure previously selected genres stay selected
                }).ToList();

                return View(model);
            }

            // Handle file upload if a new file is provided
            if (model.ImageFile != null)
            {
                var fileResult = _fileService.SaveImage(model.ImageFile);
                if (fileResult.Item1 == 0)
                {
                    TempData["msg"] = "File could not be saved.";
                    return View(model);
                }

                model.MovieImage = fileResult.Item2;
            }

            // Update the movie and associated genres
            var result = _movieService.Update(model);
            if (result)
            {
                TempData["msg"] = "Successfully updated.";
                return RedirectToAction(nameof(MovieList));
            }

            TempData["msg"] = "An error occurred while updating the movie.";
            return View(model);
        }


        public IActionResult MovieList()
        {
            var data = _movieService.List();
            return View(data);
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["msg"] = "Invalid Movie ID.";
                return RedirectToAction(nameof(MovieList));
            }

            try
            {
                // Check if the genre exists before attempting deletion
                var movie = _movieService.GetById(id);
                if (movie == null)
                {
                    TempData["msg"] = "Genre not found. Unable to delete.";
                    return RedirectToAction(nameof(MovieList));
                }

                // Attempt to delete
                var isDeleted = _movieService.Delete(id);
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

            return RedirectToAction(nameof(MovieList));
        }



    }
}
