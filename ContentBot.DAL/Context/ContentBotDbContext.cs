using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContentBot.DAL.Context
{
    public class ContentBotDbContext : IdentityDbContext<IdentityUser>
    {
        public ContentBotDbContext(DbContextOptions options) : base(options)
        {

        }

    }
}
