using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;


namespace project.Models
{

    public class MovieGenre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string genre { get; set; }
        [ForeignKey("Movie"),Column(Order = 2)]
        public Movie movie { get; set; }

        public MovieGenre(string genre, Movie movie)
        {
            this.genre = genre;
            this.movie = movie;
        }
        public MovieGenre()
        {
            this.genre = null;
            this.movie = null;
        }
    }
}
