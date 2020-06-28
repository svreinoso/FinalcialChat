using FinalcialChat.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;

namespace FinalcialChat.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Chatroom> Chatrooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatConnection> ChatConnections { get; set; }

        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }

        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            if (!modifiedEntries.Any()) return;

            var currentUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                DateTimeOffset now = DateTimeOffset.Now;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = string.IsNullOrEmpty(entity.CreatedBy) ? currentUserId : entity.CreatedBy;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.ModifiedDate = now;
                entity.ModifiedBy = currentUserId;
            }
        }

    }
}
