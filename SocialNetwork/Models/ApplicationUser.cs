using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SocialNetwork.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public virtual List<AdditionalPhoneNumber> PhoneNumbers { get; set; }

        public virtual List<AdditionalEmail> Emails { get; set; }

        public virtual List<Skype> Skypes { get; set; }

        public DateTime? Birthday { get; set; }

        [StringLength(128, ErrorMessage
            = "The Position value cannot exceed 128 characters. ")]
        public string Position { get; set; }

        public virtual List<UserToConversationLink> Links { get; set; }

        public virtual List<Resource> Resources { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}