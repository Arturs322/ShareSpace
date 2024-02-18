using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShareSpace.DataAccess.Repository.IRepository;
using ShareSpace.Models;
using System.Security.Claims;

namespace ShareSpace.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var postVM = new PostVM
            {
                Posts = _unitOfWork.Post.GetAll(includeProperties: "PostImages,User"),
            };

            if (User.Identity.IsAuthenticated)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                var userLikedPosts = _unitOfWork.PostLike.GetAll(p => p.UserId == userId).Select(p => p.PostId);

                foreach (var post in postVM.Posts)
                {
                    post.HasLiked = userLikedPosts.Contains(post.Id);
                }
            }

            return View(postVM);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult LikeAPost(int postId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            IdentityUser user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();

            var like = new PostLike
            {
                UserId = user.Id,
                PostId = postId,
            };

            var likedPost = _unitOfWork.PostLike.Get(u => u.UserId == user.Id && u.PostId == postId);
            var post = _unitOfWork.Post.Get(u => u.Id == postId);

            if (likedPost != null)
            {
                _unitOfWork.PostLike.Remove(likedPost);
                post.LikeCount--;
            } 
            else
            {
                _unitOfWork.PostLike.Add(like);
                post.LikeCount++;
            }
            
            _unitOfWork.Post.Update(post);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        #region HELPER
        public bool CheckIfUserLikedPost(int postId, string userId)
        {
            // Logic to check if the user has liked the post
            var postLike = _unitOfWork.PostLike.Get(p => p.PostId == postId && p.UserId == userId);
            return postLike != null;
        }
        #endregion
    }
}
