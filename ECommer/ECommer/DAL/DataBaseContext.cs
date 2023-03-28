using ECommer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommer.DAL
{
    public class DataBaseContext : DbContext
    {
        #region Builder
        public DataBaseContext(DbContextOptions <DataBaseContext> option) : base(option) 
        { 
        }
        #endregion

        #region Properties
        public DbSet<Country> Countries { get; set; }
        public DbSet<Category> Categories { get; set; }
        #endregion

        #region Protected methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
        }
        #endregion
    }
}
