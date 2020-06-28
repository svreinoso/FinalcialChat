namespace FinalcialChat.Migrations
{
    using FinalcialChat.Enums;
    using FinalcialChat.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Configuration;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (!roleManager.RoleExists(RolesTypes.Admin)) roleManager.Create(new IdentityRole { Name = RolesTypes.Admin });
            if (!roleManager.RoleExists(RolesTypes.User)) roleManager.Create(new IdentityRole { Name = RolesTypes.User });

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var email = ConfigurationManager.AppSettings.Get("AdminEmail");
            var exist = manager.Users.Any(u => u.Email == email);

            if (!exist)
            {
                var user = new ApplicationUser()
                {
                    UserName = email,
                    Email = email,
                    FirstName = "Samuel",
                    LastName = "Reinoso"
                };

                var password = ConfigurationManager.AppSettings.Get("AdminPassword");
                manager.Create(user, password);
                manager.AddToRole(user.Id, RolesTypes.Admin);
            }

            var chatBotName = "ChatBot1";
            var chatBot = context.Users.FirstOrDefault(x => x.UserType == UserType.Bot && x.FirstName == chatBotName);
            if(chatBot == null)
            {
                chatBot = new ApplicationUser
                {
                    UserName = chatBotName,
                    FirstName = chatBotName,
                    UserType = UserType.Bot
                };
                manager.Create(chatBot);
            }
        }
    }
}
