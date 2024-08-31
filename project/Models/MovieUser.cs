using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace project.Models;

public class MovieUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ID { get; set; }
    public string forename { get; set; }
    public string surname { get; set; }
    public string email { get; set; }
    public List<Rating> ratings { get; set; }
    public List<Tag> tags { get; set; }

    public MovieUser()
    {
        this.ID = 0;
        this.forename = null;
        this.surname = null;
        this.email = null;
        this.ratings = new List<Rating>();
        this.tags = new List<Tag>();
    }
    public MovieUser(int ID, string forename, string surname, string email)
    {
        this.ID = ID;
        this.forename = forename;
        this.surname = surname;
        this.email = email;
        this.ratings = new List<Rating>();
        this.tags = new List<Tag>();
    }

    public MovieUser(int ID, string forename, string surname, string email, List<Rating> ratings, List<Tag> tags)
    {
        this.ID = ID;
        this.forename = forename;
        this.surname = surname;
        this.email = email;
        this.ratings = ratings;
        this.tags = tags;
    }
    public MovieUser(int Id)
    {
        this.ID = Id;
        this.forename = null;
        this.surname = null;
        this.email = null;
        this.ratings = new List<Rating>();
        this.tags = new List<Tag>();
    }
    public MovieUser(int Id, string forename)
    {
        this.ID = Id;
        this.forename = forename;
        this.surname = null;
        this.email = null;
        this.ratings = new List<Rating>();
        this.tags = new List<Tag>();
    }
    public MovieUser(int Id, string forename, string surname)
    {
        this.ID = Id;
        this.forename = forename;
        this.surname = surname;
        this.email = null;
        this.ratings = new List<Rating>();
        this.tags = new List<Tag>();
    }

}