using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using project.Data;
using project.Models;

namespace project.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;

        [Flags]
        public enum Columns
        {
            Title = 0b_0000_0000_0000,
            Year = 0b_0000_0000_0001,
            Genre = 0b_0000_0000_0010,
            MedianRating = 0b_0000_0000_0100,
            AvgRating = 0b_0000_0000_1000,
            NoRatings = 0b_0000_0001_0000,
            RatingMode = 0b_0000_0010_0000,
            NewestRating = 0b_0000_0100_0000,
            OldestRating = 0b_0000_1000_0000,
            NoTags = 0b_0001_0000_0000,
            TagMode = 0b_0010_0000_0000,
            NewestTag = 0b_0100_0000_0000,
            OldestTag = 0b_1000_0000_0000
        }

        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movies
        /*public async Task<IActionResult> Index()
        {
              return _context.Movies != null ? 
                          View(await _context.Movies.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Movies'  is null.");
        }*/

        // GET: Movies
        public async Task<IActionResult> Index(string sortOrder, int columns, int page=1)
        {
            ViewData["TitleSortParam"] = String.IsNullOrEmpty(sortOrder) || sortOrder == "title" ? "title_desc" : "";
            ViewData["YearSortParam"] = sortOrder == "year" ? "year_desc" : "year";
            ViewData["GenreSortParam"] = sortOrder == "genre" ? "genre_desc" : "genre";

            ViewData["MedianRatingSortParam"] = sortOrder == "medianRating" ? "medianRating_desc" : "medianRating";
            ViewData["AvgRatingSortParam"] = sortOrder == "avgRating" ? "avgRating_desc" : "avgRating";
            ViewData["NoRatingsSortParam"] = sortOrder == "noRatings" ? "noRatings_desc" : "noRatings";
            ViewData["RatingModeSortParam"] = sortOrder == "ratingMode" ? "ratingMode_desc" : "ratingMode";
            ViewData["NewestRatingSortParam"] = sortOrder == "newestRating" ? "newestRating_desc" : "newestRating";
            ViewData["OldestRatingSortParam"] = sortOrder == "oldestRating" ? "oldestRating_desc" : "oldestRating";

            ViewData["NoTagsSortParam"] = sortOrder == "noTags" ? "noTags_desc" : "noTags";
            ViewData["TagModeTagSortParam"] = sortOrder == "tagMode" ? "tagMode_desc" : "tagMode";
            ViewData["NewestTagSortParam"] = sortOrder == "newestTag" ? "newestTag_desc" : "newestTag";
            ViewData["OldestTagSortParam"] = sortOrder == "oldestTag" ? "oldestTag_desc" : "oldestTag";
            ViewData["sortOrder"] = sortOrder;

            var movies = _context.Movies
                .Include(m => m.genres)
                .Include(m => m.ratings)
                .Include(m => m.tags)
            .AsQueryable();

            switch (sortOrder)
            {

                case "year":
                    movies = movies.OrderBy(s => s.year);
                    break;
                case "year_desc":
                    movies = movies.OrderByDescending(s => s.year);
                    break;
                case "genre":
                    movies = movies.OrderBy(s => s.genres.FirstOrDefault().genre);
                    break;
                case "genre_desc":
                    movies = movies.OrderByDescending(s => s.genres.FirstOrDefault().genre);
                    break;
                case "medianRating":
                    movies = movies.OrderBy(s => s.medianRating == null ? 1:0).ThenBy(s => s.medianRating);
                    //movies = movies.OrderByDescending(s => s.ratings.OrderBy(r => r.rating_value).ElementAt((s.ratings.Count() - 1) / 2).rating_value);
                    break;
                case "medianRating_desc":
                    movies = movies.OrderBy(s => s.medianRating == null ? 1:0).ThenByDescending(s => s.medianRating);
                    //movies = movies.OrderByDescending(sw => s.ratings.OrderBy(r => r.rating_value).ElementAt((s.ratings.Count() - 1) / 2).rating_value);
                    break;
                case "avgRating":
                    movies = movies.OrderBy(s => s.avgRating == null ? 1 : 0).ThenBy(s => s.avgRating);
                    //movies = movies.GroupBy(m => m.ratings.Average(r => r.rating_value)).OrderByDescending(m => m.Key);
                    //movies = movies.OrderBy(s => s.ratings.Average(r => r.rating_value));
                    break;
                case "avgRating_desc":
                    movies = movies.OrderBy(s => s.avgRating == null ? 1 : 0).ThenByDescending(s => s.avgRating);
                    //movies = movies.OrderByDescending(s => s.ratings.Average(r => r.rating_value));
                    break;
                case "noRatings":
                    movies = movies.OrderBy(s => s.ratings.Count());
                    break;
                case "noRatings_desc":
                    movies = movies.OrderByDescending(s => s.ratings.Count());
                    break;
                case "ratingMode":
                    movies = movies.OrderBy(s => s.ratingMode == null ? 1 : 0).ThenBy(s => s.ratingMode);
                    //movies = movies.OrderBy(s => s.ratings.GroupBy(r => r.rating_value).OrderByDescending(g => g.Count()).FirstOrDefault().FirstOrDefault().rating_value);
                    break;
                case "ratingMode_desc":
                    movies = movies.OrderBy(s => s.ratingMode == null ? 1 : 0).ThenByDescending(s => s.ratingMode);
                    //movies = movies.OrderByDescending(s => s.ratings.GroupBy(r => r.rating_value).OrderByDescending(g => g.Count()).FirstOrDefault().FirstOrDefault().rating_value);
                    break;
                case "newestRating":
                    movies = movies.OrderBy(s => s.ratings.Max(r => r.date));
                    break;
                case "newestRating_desc":
                    movies = movies.OrderByDescending(s => s.ratings.Max(r => r.date));
                    break;
                case "oldestRating":
                    movies = movies.OrderBy(s => s.ratings.Min(r => r.date));
                    break;
                case "oldestRating_desc":
                    movies = movies.OrderByDescending(s => s.ratings.Min(r => r.date));
                    break;
                case "noTags":
                    movies = movies.OrderBy(s => s.tags.Count());
                    break;
                case "noTags_desc":
                    movies = movies.OrderByDescending(s => s.tags.Count());
                    break;
                case "tagMode":
                    movies = movies.OrderBy(s => s.tagMode == null ? 1 : 0).ThenBy(s => s.tagMode);
                    //movies = movies.OrderBy(item => item.tags.GroupBy(t => t.tag_value).OrderByDescending(g => g.Count()).FirstOrDefault().FirstOrDefault().tag_value);
                    break;
                case "tagMode_desc":
                    movies = movies.OrderBy(s => s.tagMode == null ? 1 : 0).ThenByDescending(s => s.tagMode);
                    //movies = movies.OrderByDescending(s => s.tags.GroupBy(t => t.tag_value).OrderByDescending(g => g.Count()).FirstOrDefault().FirstOrDefault().tag_value);
                    break;
                case "newestTag":
                    movies = movies.OrderBy(s => s.tags.Max(t => t.date));
                    break;
                case "newestTag_desc":
                    movies = movies.OrderByDescending(s => s.tags.Max(t => t.date));
                    break;
                case "oldestTag":
                    movies = movies.OrderBy(s => s.tags.Min(t => t.date));
                    break;
                case "oldestTag_desc":
                    movies = movies.OrderByDescending(s => s.tags.Min(t => t.date));
                    break;
                case "title_desc":
                    movies = movies.OrderByDescending(s => s.title);
                    break;
                default:
                    movies = movies.OrderBy(s => s.title);
                    break;
            }
            ViewData["page"] = page;
            return View(await movies.Skip((page-1)*50).Take(50).AsNoTracking().ToListAsync());
            

        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,title,year")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,title,year")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Movies'  is null.");
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
