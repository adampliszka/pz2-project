using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using project.Models;
using System.Globalization;
using System.Reflection.Emit;
using Microsoft.Extensions.DependencyInjection;
using project.Data;
using NuGet.Packaging.Signing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CsvHelper;

namespace project.Models
{
    public class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var _context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                //_context.Movies.RemoveRange(_context.Movies);
                /*_context.Movies.RemoveRange(_context.Movies);
                _context.MovieGenres.RemoveRange(_context.MovieGenres);
                _context.MovieUsers.RemoveRange(_context.MovieUsers);
                _context.Ratings.RemoveRange(_context.Ratings);
                _context.Tags.RemoveRange(_context.Tags);
                await _context.SaveChangesAsync();
                //*/
                if (!_context.Movies.Any())
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true,
                        Escape = '\\',
                        HeaderValidated = null,
                        Delimiter = ",",
                        Mode = CsvMode.RFC4180,
                        TrimOptions = TrimOptions.InsideQuotes,
                        NewLine = "\r\n",
                        InjectionOptions = InjectionOptions.None,
                        Encoding = System.Text.Encoding.UTF8,
                        AllowComments = true
                    };

                    using (var reader = new StreamReader("movies.csv"))
                    using (var csv = new CsvHelper.CsvReader(reader, config))
                    {
                        _context.MovieGenres.RemoveRange(_context.MovieGenres);
                        var anonType = new
                        {
                            movieId = default(int),
                            title = string.Empty,
                            genres = string.Empty
                        };
                        var movies = csv.GetRecords(anonType).ToList();
                        foreach (var movie in movies)
                        {
                            var m = new Movie(movie.movieId, movie.title, movie.genres);
                            _context.Movies.Add(m);
                            foreach (var g in m.genres)
                            {
                                _context.MovieGenres.Add(g);
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
                /*------------------------------------------------------------*/

                //applicationDbContext.MovieUsers.RemoveRange(applicationDbContext.MovieUsers);
                if (!_context.MovieUsers.Any())
                {
                    using (var reader = new StreamReader("users.csv"))
                    using (var csv = new CsvHelper.CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        var anonType = new
                        {
                            userId = default(int),
                            foreName = string.Empty,
                            surName = string.Empty,
                            email = string.Empty
                        };
                        var users = csv.GetRecords(anonType).ToList();
                        foreach (var user in users)
                        {
                            var m = new MovieUser(user.userId, user.foreName, user.surName, user.email);
                            _context.MovieUsers.Add(m);
                        }
                    }
                }
                await _context.SaveChangesAsync();
                /*------------------------------------------------------------*/
                if (!_context.Ratings.Any())
                {
                    using (var reader = new StreamReader("ratings.csv"))
                    using (var csv = new CsvHelper.CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        var anonType = new
                        {
                            userId = default(int),
                            movieId = default(int),
                            rating = default(double),
                            timestamp = default(long),
                        };
                        var ratings = csv.GetRecords(anonType).ToList();
                        foreach (var rating in ratings)
                        {
                            var user = _context.MovieUsers.FirstOrDefault(m => m.ID == rating.userId);
                            var movie = _context.Movies.FirstOrDefault(m => m.Id == rating.movieId);
                            var newRating = new Rating(user, movie, rating.rating, rating.timestamp);
                            user.ratings.Append(newRating);
                            movie.ratings.Append(newRating);
                            //movie.ratings = movie.ratings;
                            _context.Ratings.Add(newRating);
                            movie.ratings = movie.ratings;//hack to update the rating list, to be removed
                        }
                    }
                }
                await _context.SaveChangesAsync();
                /*------------------------------------------------------------*/

                if (!_context.Tags.Any())
                {
                    using (var reader = new StreamReader("tags.csv"))
                    using (var csv = new CsvHelper.CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        var anonType = new
                        {
                            userId = default(int),
                            movieId = default(int),
                            tag = string.Empty,
                            timestamp = default(long),
                        };
                        var tags = csv.GetRecords(anonType).ToList();
                        foreach (var tag in tags)
                        {
                            var user = _context.MovieUsers.FirstOrDefault(m => m.ID == tag.userId);
                            var movie = _context.Movies.FirstOrDefault(m => m.Id == tag.movieId);
                            var newTag = new Tag(user, movie, tag.tag, tag.timestamp);
                            user.tags.Append(newTag);
                            movie.tags.Append(newTag);
                            //movie.tags= movie.tags;
                            _context.Tags.Add(newTag);
                            movie.tags = movie.tags;//hack to update the tag list, to be removed
                        }
                    }
                }
                await _context.SaveChangesAsync();
                /*------------------------------------------------------------*/

                var rm = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!rm.Roles.Any())
                {
                    var r = await rm.CreateAsync(new IdentityRole("Admin"));
                    //Debug.WriteLine("https://" + r.Result);
                    await rm.CreateAsync(new IdentityRole("User"));
                }

                var manager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                if (!manager.Users.Any())
                {

                    var user = new IdentityUser { Email = "admin@admin.com", UserName = "admin@admin.com"};
                    var result = await manager.CreateAsync(user, "Adminadmin123!");
                    //Debug.WriteLine("https://"+result.Result);
                    await manager.ConfirmEmailAsync(user, manager.GenerateEmailConfirmationTokenAsync(user).Result);
                    await manager.AddToRoleAsync(user, "User");
                    await manager.AddToRoleAsync(user, "Admin");
                    await manager.SetLockoutEnabledAsync(user, false);

                    var user1 = new IdentityUser { Email = "user@user.com", UserName = "user@user.com" };
                    var result1 = await manager.CreateAsync(user1, "Useruser123!");
                    //Debug.WriteLine("https://" + result.Result);
                    await manager.ConfirmEmailAsync(user1, manager.GenerateEmailConfirmationTokenAsync(user1).Result);
                    await manager.AddToRoleAsync(user1, "User");
                    await manager.SetLockoutEnabledAsync(user1, false);
                }

                /*------------------------------------------------------------*/

                await _context.SaveChangesAsync();
            }
        }
    }
}