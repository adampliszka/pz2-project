using project.Models;

namespace project
{
    public class RatingDTO
    {
        public int ID { get; set; }
        public int userID { get; set; }
        public int movieID { get; set; }
        public double rating_value { get; set; }
        public long timestamp { get; set; }
        public RatingDTO(int user, int movie, double rating, long timestamp)
        {
            this.userID = user;
            this.movieID = movie;
            this.rating_value = rating;
            this.timestamp = timestamp;
        }
    }
}
