using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShareSpace.Models
{
    public class PostLike
    {
        [Key]
        public int Id { get; set; }
        [ValidateNever]
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        [ValidateNever]
        public Post Post { get; set; }
        [ValidateNever]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }
    }
}