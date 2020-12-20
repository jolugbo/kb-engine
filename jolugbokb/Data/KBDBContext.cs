using System;
using jolugbokb.Models;
using Microsoft.EntityFrameworkCore;

namespace jolugbokb.Data
{
    public class KBDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPass> UserSecure { get; set; }
        public virtual DbSet<SessionKey> SessionKey { get; set; }
        public KBDBContext(DbContextOptions<KBDBContext> options) : base(options) {

        }
    }
}
