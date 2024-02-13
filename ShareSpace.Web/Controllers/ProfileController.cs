using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShareSpace.DataAccess.Repository.IRepository;
using ShareSpace.Models;
using System.Security.Claims;

namespace ShareSpace.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index(string? userId)
        {
            if(userId == null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            ApplicationUser user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();

            var postVM = new PostVM
            {
                Posts = _unitOfWork.Post.GetAll(u => u.AuthorId == userId, includeProperties: "PostImages"),
                User = user,
                PostCount = _unitOfWork.Post.GetAll(u => u.AuthorId == userId).Count(),
            };

            return View(postVM);
        }

    }
}
