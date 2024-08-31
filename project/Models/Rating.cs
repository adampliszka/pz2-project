using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace project.Models;

public class Rating
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public MovieUser user { get; set; }
    public Movie movie { get; set; }
    public double? rating_value { get; set; }
    public DateTime? date { get; set; }
    
    public Rating(MovieUser user, Movie movie, double rating, long timestamp)
    {
        this.user = user;
        this.movie = movie;
        this.rating_value = rating;
        var offset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
        this.date = offset.UtcDateTime;
    }
    public Rating(MovieUser user, Movie movie, double rating, DateTime date)
    {
        this.user = user;
        this.movie = movie;
        this.rating_value = rating;
        this.date = date;
    }
    public Rating()
    {
        this.user = null;
        this.movie = null;
        this.rating_value = null;
        this.date = null;
    }
}