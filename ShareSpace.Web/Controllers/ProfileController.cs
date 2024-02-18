using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult Index(string? userId)
        {
            if(userId == null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            ApplicationUser user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();

            var following = _unitOfWork.Follow.Get(u => u.FollowUserId == userId);
            var postVM = new PostVM
            {
                Posts = _unitOfWork.Post.GetAll(u => u.AuthorId == userId, includeProperties: "PostImages"),
                User = user,
                PostCount = _unitOfWork.Post.GetAll(u => u.AuthorId == userId).Count(),
                FollowersCount = _unitOfWork.Follow.GetAll(u => u.UserId == userId).Count(),
                FollowingCount = _unitOfWork.Follow.GetAll(u => u.FollowUserId == userId).Count(),
            };

            return View(postVM);
        }

        [HttpGet]
        public IActionResult Followers(string? userProfileId)
        {
            ApplicationUser userProfile = _userManager.FindByIdAsync(userProfileId).GetAwaiter().GetResult();

            var followers = new ProfileVM
            {
                Followers = _unitOfWork.Follow.GetAll(u => u.UserId == userProfile.Id, includeProperties: "FollowUser"),
                User = userProfile,
            };

            return View(followers);
        }

        [HttpGet]
        public IActionResult Following(string? userProfileId)
        {
            ApplicationUser userProfile = _userManager.FindByIdAsync(userProfileId).GetAwaiter().GetResult();

            var followers = new ProfileVM
            {
                Followers = _unitOfWork.Follow.GetAll(u => u.FollowUserId == userProfile.Id, includeProperties: "User"),
                User = userProfile,
            };

            return View(followers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Follow(string? followUserId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser currentUser = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();

            ApplicationUser followUser = _userManager.FindByIdAsync(followUserId).GetAwaiter().GetResult();

            var existingFollow = _unitOfWork.Follow.Get(u => u.UserId == followUserId && u.FollowUserId == userId);

            var follow = new Follow
            {
                UserId = followUser.Id, //user that receives a follow
                FollowUserId = currentUser.Id //current user that follows other user
            };

            if (existingFollow != null)
            {
                _unitOfWork.Follow.Remove(existingFollow);
            }
            else
            {
                _unitOfWork.Follow.Add(follow);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index), new { userId = followUserId });
        }
    }
}
