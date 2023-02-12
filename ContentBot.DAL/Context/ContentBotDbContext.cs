using ContentBot.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContentBot.DAL.Context
{
    public class ContentBotDbContext : IdentityDbContext<ApplicationUser>
    {
        public ContentBotDbContext(DbContextOptions options) : base(options)
        {

        }

    }
}
