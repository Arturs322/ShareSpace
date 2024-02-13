namespace ShareSpace.Models
{
    public class PostVM
    {
        public IEnumerable<Post> Posts { get; set; }
        public ApplicationUser User { get; set; }
        public int PostCount {  get; set; }
    }
}