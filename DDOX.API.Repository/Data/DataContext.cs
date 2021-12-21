using DDOX.API.Infrastructure.Configurations;
using DDOX.API.Infrastructure.Models;
using DDOX.API.Repository.Configuration;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Repository.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Partition> Partition { get; set; }
        public DbSet<Restriction> Restriction { get; set; }
        public DbSet<Infrastructure.Models.Index> Index { get; set; }
        public DbSet<Folder> Folder { get; set; }
        public DbSet<File> File { get; set; }
        public DbSet<Extension> Extension { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<IndexRestrictions> IndexRestrictions { get; set; }
        public DbSet<CategoryIndices> CategoryIndices { get; set; }
        public DbSet<FolderIndices> FolderIndices { get; set; }
        public DbSet<FolderCategories> FolderCategories { get; set; }
        public DbSet<FileIndices> FileIndices { get; set; }
        public DbSet<Page> Page { get; set; }
        public DbSet<PageIndices> PageIndices { get; set; }
         

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder>()
                .HasOne(f => f.ParentFolder)
                .WithMany(f => f.SubFolders)
                .HasForeignKey(f => f.ParentId);

            try
            {
                modelBuilder.ApplyConfiguration(new ExtentionConfiguration());
                modelBuilder.ApplyConfiguration(new RestrictionConfiguration());
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error While seeding initial data");
            }

        }



    }
}
