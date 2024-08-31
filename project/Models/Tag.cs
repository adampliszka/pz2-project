using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace project.Models;


public class Tag

{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Key, Column(Order = 0)]
    public Movie movie { get; set; }
    [Key, Column(Order = 1)]
    public MovieUser user { get; set; }
    public DateTime? date { get; set; }
    public string tag_value { get; set; }

    public Tag(MovieUser user, Movie movie, string tag, long timestamp)
    {
        this.user = user;
        this.movie = movie;
        this.tag_value = tag;
        var offset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
        this.date = offset.UtcDateTime;
    }

    public Tag(MovieUser user, Movie movie, string tag_value, DateTime date)
    {
        this.user = user;
        this.movie = movie;
        this.tag_value = tag_value;
        this.date = date;
    }
    public Tag()
    {
        this.user = null;
        this.movie = null;
        this.tag_value = null;
        this.date = null;
    }

}