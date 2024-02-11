using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShareSpace.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [ValidateNever]
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime PostDate { get; set; } = DateTime.Now;
        public int? LikeCount { get; set; }
        [ValidateNever]
        public List<PostImage> PostImages { get; set; }
    }
}