using ContentBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentBot.DAL.Context
{
    public class ContentBotDbContext : DbContext
    {
        public ContentBotDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Contact> Contacts { get; set; }
    }
}
