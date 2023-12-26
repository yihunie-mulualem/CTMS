using CTMS.Models;
using Microsoft.EntityFrameworkCore;

namespace CTMS.DatabaseContext
{
    public class CTMSDbContext : DbContext
    {
        public CTMSDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData
                (
                
                new Role() { Id = 1, Name = "ADMIN"},
                new Role() { Id = 2, Name = "USER" }
               // new Role() { Id = 3, Name = "MANAGER"}
               );
            modelBuilder.Entity<User>().HasData
                (
                
                new User() { Id = 1,FullName = "ADMIN",UserName="Admin",Email="admin@berhanbank.com",Password= "MTIz",viewStatus=true,RoleId = 1 }
               );

        }

        public DbSet<ApplicationCase> ApplicationCases { get; set; }
        public DbSet<ApplicationName> ApplicationNames { get; set; }
        //public DbSet<ChangeType> ChangeTypes { get; set; }
        public DbSet<DatabaseCase> DatabaseCases { get; set; }
        public DbSet<Department> Departments { get; set; }
        //public DbSet<FileUpload> fileUploads { get; set; }
       // public DbSet<Programer> Programers { get; set; }
        public DbSet<RequestForm> RequestForms { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

    }


}
