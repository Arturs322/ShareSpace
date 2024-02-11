using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareSpace.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string StreetAdress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public int Followers { get; set; }
        [ValidateNever]
        public int Following { get; set; }
        [ValidateNever]
        public int Posts { get; set; }
        public string ProfilePictureUrl { get; set; } // Property to hold the URL or path of the profile picture
        [NotMapped]
        public string Role { get; set; }
    }
}