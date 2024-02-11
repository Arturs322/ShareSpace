using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ShareSpace.Models
{
    public class PostVM
    {
        public IEnumerable<Post> Posts { get; set; }
    }
}