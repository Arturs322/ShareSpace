using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShareSpace.Models
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }
        [ValidateNever]
        public string FollowUserId { get; set; }
        [ForeignKey("FollowUserId")]
        [ValidateNever]
        public ApplicationUser FollowUser { get; set; }
        [ValidateNever]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }
    }
}