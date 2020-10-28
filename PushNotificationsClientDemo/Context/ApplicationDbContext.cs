using Microsoft.EntityFrameworkCore;
using PushNotificationsClientDemo.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PushNotificationsClientDemo.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Setting> Settings { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=WebPushDemoDb;User Id=sa;Password=P@ssw0rd;MultipleActiveResultSets=True;");
            }
        }
    }
}
