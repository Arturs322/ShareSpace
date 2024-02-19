namespace ShareSpace.Models
{
    public class ProfileVM
    {
        public IEnumerable<Follow> Followers { get; set; }
        public ApplicationUser User { get; set; }
    }
}