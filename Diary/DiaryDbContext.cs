using Diary.Models.Configurations;
using Diary.Models.Domains;
using System;
using System.Data.Entity;
using System.Linq;

namespace Diary
{
    public class DiaryDbContext : DbContext
    {
        public DiaryDbContext()
            : base($"Server={DBConfig.Server};Database={DBConfig.Database};User Id={DBConfig.Id}; Password={DBConfig.Password};")
        {
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GroupConfiguration());
            modelBuilder.Configurations.Add(new RatingConfiguration());
            modelBuilder.Configurations.Add(new StudentConfiguration());
        }
    }

}