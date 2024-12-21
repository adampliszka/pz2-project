using project.Models;

namespace project
{
    public class TagDTO
    {
        public int ID { get; set; }
        public int userID { get; set; }
        public int movieID { get; set; }
        public string tag_value { get; set; }
        public long timestamp { get; set; }
        public TagDTO(int user, int movie, string tag, long timestamp)
        {
            this.userID = user;
            this.movieID = movie;
            this.tag_value = tag;
            this.timestamp = timestamp;
        }
    }
}