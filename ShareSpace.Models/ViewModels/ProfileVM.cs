namespace ShareSpace.Models
{
    public class ProfileVM
    {
        public IEnumerable<Follow> Followers { get; set; }
        public ApplicationUser User { get; set; }
        public int PostCount {  get; set; }
        public int FollowersCount {  get; set; }
        public bool Following { get; set; }
    }
}