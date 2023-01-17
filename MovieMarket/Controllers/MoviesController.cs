using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using MovieMarket.Models;
using MovieStore.Models;
using MovieStore.ViewModel;
using NToastNotify;

namespace MovieStore.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDBContext _context;
        private IToastNotification _toastrNotify;

        private int _megabyte = 1048576;
        private List<String> _allowedExtensions = new List<String> { ".jpg", ".png" };

        public MoviesController(ApplicationDBContext context , IToastNotification toastrNotify)
        {
            _context = context;
            _toastrNotify = toastrNotify;
        }
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies.OrderByDescending(m => m.Rate).ToListAsync();

            return View(movies);
        }



        public async Task<IActionResult> Create()
        {
            var movieform = new MovieFormViewModel
            {
                Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync()
            };
            return View("MovieForm", movieform);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel movieModel)
        {
            //ModelState["Poster"].ValidationState = ModelValidationState.Valid;
            //ModelState["Genres"].ValidationState = ModelValidationState.Valid;


            if (!ModelState.IsValid)
            {
                //var errors = ModelState.Values.SelectMany(v => v.Errors);
                //String erormsg = "";
                //foreach (var er in errors)
                //{
                //    erormsg += er.ErrorMessage;
                //    erormsg += "///";

                //}
                //ModelState.AddModelError("Name", erormsg);
                movieModel.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();

                return View("MovieForm", movieModel);
            }
            var files = Request.Form.Files;
            if (!files.Any())
            {
                movieModel.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Must Select a Poster!");
                return View("MovieForm", movieModel);

            }
            var poster = files.FirstOrDefault();
            if (!_allowedExtensions.Contains(Path.GetExtension(poster.FileName.ToLower())))
            {
                movieModel.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();

                ModelState.AddModelError("Poster", "Only JPG and PNG images are allowed!");
                return View("MovieForm", movieModel);

            }
            if (poster.Length > 3 * _megabyte)
            {
                movieModel.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "The image size can not exceed 3 MB!");
                return View("MovieForm", movieModel);
            }
            using var datastream = new MemoryStream();
            await poster.CopyToAsync(datastream);
            var movie = new Movie
            {
                Name = movieModel.Name,
                GenreId = movieModel.GenreId,
                Year = movieModel.Year,
                Rate = movieModel.Rate,
                StoryLine = movieModel.StoryLine,
                Poster = datastream.ToArray(),
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();
            _toastrNotify.AddSuccessToastMessage("Movie added successfully!");
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();
            var MovieView = new MovieFormViewModel
            {
                Id = movie.Id,
                Rate = movie.Rate,
                Poster = movie.Poster,
                Name = movie.Name,
                Year = movie.Year,
                StoryLine = movie.StoryLine,
                GenreId = movie.GenreId,
                Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync()

            };
            return View("MovieForm", MovieView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel movieModel)
        {
            if (!ModelState.IsValid)
            {
                movieModel.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                return View("MovieForm",movieModel);
            }



            var movie = await _context.Movies.FindAsync(movieModel.Id);
            if (movie == null)
                return NotFound();


            var files = Request.Form.Files;
            if (files.Any())
            {
                var poster = files.FirstOrDefault();
                using var datastream = new MemoryStream();
                await poster.CopyToAsync(datastream);

                movieModel.Poster = datastream.ToArray();

                if (!_allowedExtensions.Contains(Path.GetExtension(poster.FileName.ToLower())))
                {
                    movieModel.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();

                    ModelState.AddModelError("Poster", "Only JPG and PNG images are allowed!");
                    return View("MovieForm", movieModel);

                }
                if (poster.Length > 3 * _megabyte)
                {
                    movieModel.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "The image size can not exceed 3 MB!");
                    return View("MovieForm", movieModel);
                }

                movie.Poster = movieModel.Poster; // Or datastream.ToArray();

            }


            movie.Name = movieModel.Name;
            movie.GenreId = movieModel.GenreId;
            movie.Year = movieModel.Year;   
            movie.StoryLine = movieModel.StoryLine; 
            movie.GenreId = movieModel.GenreId;
            movie.Rate = movieModel.Rate;
           
            

            _context.SaveChanges();
            _toastrNotify.AddSuccessToastMessage("Movie updated successfully!");

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();
            var movie = await _context.Movies.Include(m=> m.Genre).SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return NotFound();

            return View(movie);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            _context.Movies.Remove(movie);  
            _context.SaveChanges(); 
            return Ok();
        }
    }
}
