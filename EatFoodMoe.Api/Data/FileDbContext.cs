using EatFoodMoe.Api.Entitles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatFoodMoe.Api.Data
{
    public class FileDbContext : DbContext
    {
        public FileDbContext(DbContextOptions<FileDbContext> options)
            : base(options)
        {

        }

        public DbSet<EatFoodFile> EatFoodFiles { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<SinicizationGroup> SinicizationGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var groupId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            builder.Entity<SinicizationGroup>().HasData(new SinicizationGroup[]
            {
                new SinicizationGroup
                {
                    Id = groupId,
                    Nmae = "Unknowed"
                }
            });
            builder.Entity<Project>().HasData(new Project[]
            {
                new Project
                {
                    Id = projectId,
                    Name = "Unknowed",
                    GameName = "Unknowed",
                    MemberNames = "Unknowed",
                    FileCount = 0,
                    GroupId = groupId
                }
            });
        }

    }
}
