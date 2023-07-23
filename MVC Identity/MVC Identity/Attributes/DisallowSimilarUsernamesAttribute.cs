using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MVC_Identity.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DisallowSimilarUsernamesAttribute : ValidationAttribute
    {
        private readonly UserManager<AppUser> _userManager;
        public DisallowSimilarUsernamesAttribute()
        {
                
        }
        public DisallowSimilarUsernamesAttribute(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var usernameExists = _userManager.Users.Any(user => user.UserName.Equals(value));
            if (value != null && usernameExists)
            {
                return new ValidationResult($"The username {value} already exists.");
            }
            return ValidationResult.Success;
        }
    }
}
