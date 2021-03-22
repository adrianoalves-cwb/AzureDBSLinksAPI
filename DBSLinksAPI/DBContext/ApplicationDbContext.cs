using System;
using DBSLinksAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DBSLinksAPI.DBContext
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<Login> Logins { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationCategory> ApplicationCategories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<DealerContact> DealerContacts { get; set; }
    }
}
