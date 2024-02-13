using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

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
