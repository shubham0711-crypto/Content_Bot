using ContentBot.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContentBot.DAL.Context
{
    public class ContentBotDbContext : IdentityDbContext<ApplicationUser, Roles, string>
    {
        public ContentBotDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ApplicationUser> applicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Roles>().Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Entity<Roles>().HasKey(p => p.Id);

            builder.Entity<Roles>().Property(a => a.ConcurrencyStamp)
           .IsConcurrencyToken(true).ValueGeneratedOnAddOrUpdate();

            builder.Entity<ApplicationUser>().Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Entity<ApplicationUser>().HasKey(p => p.Id);

            builder.Entity<ApplicationUser>().Property(a => a.ConcurrencyStamp)
           .IsConcurrencyToken(true).ValueGeneratedOnAddOrUpdate();


            //builder.Entity<Role>().Property(x => x.Id).IsConcurrencyToken(true);            
        }

    }
}
