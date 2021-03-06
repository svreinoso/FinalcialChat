﻿using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Hosting;
using FinalcialChat.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinalcialChat.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string  FirstName { get; set; }
        public string  LastName { get; set; }
        public UserType UserType { get; set; }

        public virtual ICollection<Chatroom> Chatrooms { get; set; }
        public virtual ICollection<ChatConnection> Connections { get; set; }
    }
}
