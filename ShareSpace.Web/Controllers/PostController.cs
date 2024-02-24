using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShareSpace.DataAccess.Repository.IRepository;
using ShareSpace.Models;
using System.Security.Claims;

namespace ShareSpace.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PostController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post post, List<IFormFile> files)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = (ApplicationUser)_userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
            post.AuthorId = user.Id;
            _unitOfWork.Post.Add(post);
            _unitOfWork.Save();

            if (files != null)
            {
                foreach (IFormFile file in files)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = @"images\posts\post-" + post.Id;
                    string finalPath = Path.Combine(wwwRootPath, productPath);

                    if (!Directory.Exists(finalPath))
                    {
                        Directory.CreateDirectory(finalPath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    var productImage = new PostImage
                    {
                        imageUrl = @"\" + productPath + @"\" + fileName,
                        PostId = post.Id,
                    };

                    if (post.PostImages == null)
                        post.PostImages = new List<PostImage>();

                    post.PostImages.Add(productImage);
                }

                _unitOfWork.Post.Update(post);
                _unitOfWork.Save();
            }


            TempData["success"] = "Post Added";
            return RedirectToAction(nameof(Create));
        }

        [HttpPost]
        [Authorize]
        public JsonResult Comment(int postId, string comment)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();

            var userComment = new PostComment
            {
                Comment = comment,
                AuthorId = user.Id,
                PostId = postId,
            };
            _unitOfWork.PostComment.Add(userComment);
            _unitOfWork.Save();

            return Json(new
            {
                success = true,
                newComment = comment,
                userName = user.Name,
                userId = user.Id,
                profilePictureUrl = user.ProfilePictureUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int postId)
        {
            var postToDelete = _unitOfWork.Post.Get(u => u.Id == postId);
            if (postToDelete != null)
            {
                _unitOfWork.Post.Remove(postToDelete);
                _unitOfWork.Save();
                TempData["success"] = "Post Deleted Successfully!";
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
