using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Users
{
    public class RegisterUserModel
    {
        //public string ErrorMessage { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "A username is required.")]
        [MinLength(4, ErrorMessage = "Your username must be at least 4 characters.")]
        [MaxLength(20, ErrorMessage = "Your username cannot exceed 20 characters.")]
        public string UserName { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Your email address is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Confirm Email Address")]
        [Compare(nameof(Email), ErrorMessage = "Your confirmation email must match.")]
        public string ConfirmEmail { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "A password is required.")]
        // todo: Enable password data type
        //[DataType(DataType.Password)] // todo: Uncomment password data types.
        [MinLength(10, ErrorMessage = "Your password must be at least 10 characters.")]
        [MaxLength(50, ErrorMessage = "Your password cannot exceed 50 characters.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        // todo: Enable password data type
        //[DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Your passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Your phone number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Your phone number must be 10 numbers.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "Your zip code is required.")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Your zip code must be 5 numbers.")]
        public string ZipCode { get; set; }
    }
}