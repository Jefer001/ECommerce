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
        #endregion

        #region Protected methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
        }
        #endregion
    }
}
