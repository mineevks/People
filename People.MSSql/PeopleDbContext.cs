using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using People.Models.Sql;
using People.Models.V1.CommonModels;
using Utilities;

namespace People.MSSql
{


    public class PeopleDbContext : DbContext
    {
        //private readonly IConfiguration _configuration;

        public PeopleDbContext(DbContextOptions<PeopleDbContext> options) : base(options) { }


        public DbSet<CitizenSql> Citizens { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CitizenSql>().HasIndex(b => b.Name);
            modelBuilder.Entity<CitizenSql>().HasIndex(p => p.Surname);
            modelBuilder.Entity<CitizenSql>().HasIndex(p => p.Patronymic);
            modelBuilder.Entity<CitizenSql>().HasIndex(p => p.Inn);
            modelBuilder.Entity<CitizenSql>().HasIndex(p => p.Snils);
            modelBuilder.Entity<CitizenSql>().HasIndex(p => p.DateOfBirth);
            modelBuilder.Entity<CitizenSql>().HasIndex(p => p.DateOfDeath);

            base.OnModelCreating(modelBuilder);
        }

        
        
        


    }

}
