using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace project.Models;

public class Movie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string title { get; set; }
    public int? year { get; set; }
    public string genreString { get; set; }
    public List<MovieGenre> genres { get; set; }
    List<Tag> _tags;
    public List<Tag> tags 
    { 
        get => this._tags; 
        set 
        { 
            this._tags = value;
            if (tags.Any())
                tagMode = value.GroupBy(i => i.tag_value).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
            else
                tagMode = null;
        } 
    }

    List<Rating> _ratings;
    public List<Rating> ratings { 
        get => this._ratings; 
        set 
        { 
            this._ratings = value;
            if (ratings.Any())
            {
                ratingMode = value.GroupBy(i => i.rating_value).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
                avgRating = value.Average(i => i.rating_value);
                medianRating = value.OrderBy(i => i.rating_value).ElementAt(Math.Abs(ratings.Count / 2)).rating_value;
            }
            else
            {
                ratingMode = null;
                avgRating = null;
                medianRating = null;
            }
        } 
    }
    public string? tagMode { get; set; } // todo: property, nie trzeba ale byłoby fajnie
    public double? ratingMode { get; set; } //todo
    public double? avgRating { get; set; } //todo
    public double? medianRating { get; set; } //todo
    public void setGenreString() //todo: convert to property
    {
        string s = "";
        foreach (var g in genres)
        {
            s += g.genre + "|";
        }
        if (s.Length > 0)
            this.genreString = s.Substring(0, s.Length - 1);
        else
            this.genreString = "";
    }
    public Movie(int movieId, string title, string genres)
    {
        this.Id = movieId;
        int x;
        if (Int32.TryParse(title.Substring(title.Length - 5, 4), out x))
            {
            this.year = x;
            this.title = title.Substring(0, title.Length - 7);
            }
        else
            {
            this.year = null;
            this.title = title;
            }
        List<MovieGenre> gnr = new List<MovieGenre>();
        if (genres != "(no genres listed)")
        {
            var movieGenresList = genres.Split('|').ToList();
            foreach (var g in movieGenresList)
            {
                MovieGenre mg = new MovieGenre(g, this);
                gnr.Add(mg);
            }
            
            this.genreString = genres;
        }
        else
        {
            this.genreString = "";
        }
        this.genres = gnr;
        this.ratings = new List<Rating>();
        this.tags= new List<Tag>();
    }
    public Movie(int movieId, string title, List<MovieGenre> genres, List<Tag> tags, List<Rating> ratings)
    {
        this.Id = movieId;
        this.title = title.Substring(0, title.Length - 7);
        this.year = Int32.Parse(title.Substring(title.Length - 5, 4));
        this.genres = genres;
        this.tags = tags;
        this.ratings = ratings;
        setGenreString();
    }
    public Movie(int movieId, string title)
    {
        this.Id = movieId;
        this.title = title.Substring(0, title.Length - 7);
        this.year = Int32.Parse(title.Substring(title.Length - 5, 4));
        this.genres = new List<MovieGenre>();
        this.ratings = new List<Rating>();
        this.tags = new List<Tag>();
        setGenreString();
    }
    public Movie()
    {
        this.Id = 0;
        this.title = null;
        this.year = null;
        this.genres = new List<MovieGenre>();
        this.ratings = new List<Rating>();
        this.tags = new List<Tag>();
        setGenreString();
    }
    public List<int> getRatingsNumeric()
    {
        List<int> ratings = new List<int>();
        foreach (var r in this.ratings)
        {
            ratings.Add((int)r.rating_value);
        }
        return ratings;
    }
}