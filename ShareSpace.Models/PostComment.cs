using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShareSpace.Models
{
    public class PostComment
    {
        [Key]
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; } = DateTime.Now;
        [ValidateNever]
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        [ValidateNever]
        public Post Post { get; set; }
        [ValidateNever]
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }

    }
}