// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShareSpace.Models;
using System.ComponentModel.DataAnnotations;

namespace ShareSpace.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string StreetAddress { get; set; }
            public string PostalCode { get; set; }
            public string Description { get; set; }
            public IFormFile ProfilePicture { get; set; }
            public string ProfilePictureUrl { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            // Assuming GetNameAsync, GetSurnameAsync, and GetImageAsync are methods you added
            var name = user.Name;
            var city = user.City;
            var state = user.State;
            var streetAddress = user.StreetAdress;
            var postalCode = user.PostalCode;
            var description = user.Description;
            var picture = user.ProfilePicture;
            var pictureUrl = user.ProfilePictureUrl;

            // Get current claims of the user
            var claims = await _userManager.GetClaimsAsync(user);

            // Check if the user already has a specific claim
            var nameClaim = claims.FirstOrDefault(c => c.Type == "Name");

            // Similar checks for other claims

            // Store the ApplicationUser instance into user2 (assuming you have it declared)
            ApplicationUser user2 = user;

            Input = new InputModel
            {
                Name = name,
                PhoneNumber = phoneNumber,
                City = city,
                State = state,
                StreetAddress = streetAddress,
                PostalCode = postalCode,
                Description = description,
                ProfilePicture = picture,
                ProfilePictureUrl = pictureUrl
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Redirect the user to the login page or return an error message.
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User) as ApplicationUser;
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "identity" });
            }

            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return RedirectToPage();
                }
            }

            if (Input.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = Input.PhoneNumber;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return RedirectToPage();
                }
            }

            if (Input.City != user.City)
            {
                user.City = Input.City;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return RedirectToPage();
                }
            }

            if (Input.State != user.State)
            {
                user.State = Input.State;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return RedirectToPage();
                }
            }

            if (Input.StreetAddress != user.StreetAdress)
            {
                user.StreetAdress = Input.StreetAddress;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return RedirectToPage();
                }
            }

            if (Input.PostalCode != user.PostalCode)
            {
                user.PostalCode = Input.PostalCode;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return RedirectToPage();
                }
            }

            if (Input.Description != user.Description)
            {
                user.Description = Input.Description;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return RedirectToPage();
                }
            }

            if (Input.ProfilePicture != null)
            {
                string profilePictureUrl = await SaveProfilePictureAsync(user, Input.ProfilePicture);
                user.ProfilePictureUrl = profilePictureUrl;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return RedirectToPage();
                }
            }

            TempData["success"] = "Profile Updated Successfully!";
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }

        public async Task<string> SaveProfilePictureAsync(ApplicationUser user, IFormFile profilePicture)
        {
            try
            {
                // Retrieve the old profile picture URL
                string oldProfilePictureUrl = user.ProfilePictureUrl;

                // If there's an old profile picture, delete it
                if (!string.IsNullOrEmpty(oldProfilePictureUrl))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, oldProfilePictureUrl.TrimStart('/'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);

                var imagePath = Path.Combine(_environment.WebRootPath, "images", "profile-pictures", fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(imagePath));

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                return "/images/profile-pictures/" + fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving profile picture: " + ex.Message);
                throw;
            }
        }


    }
}
