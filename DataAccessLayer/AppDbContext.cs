using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace EntityFrameworkeCookBook.DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        private ISet<IPropConvention> conventions = new HashSet<IPropConvention>();

        public DbSet<User> Users { get; set; }  
        public DbSet<Address> addresses { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
            conventions.Add(StringConvention.Instance);
        }

        protected AppDbContext()
        {
            conventions.Add(StringConvention.Instance);
        }

        private void applyConvention(ModelBuilder modelBuilder)
        {
            foreach (var conv in conventions)
            {
                conv.Apply(modelBuilder);
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            applyConvention(modelBuilder);

            modelBuilder.Entity<Address>()
                .HasKey(a => a.userId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.address)
                .WithOne(a => a.user)
                .HasForeignKey<Address>(a => a.userId)
                .OnDelete(DeleteBehavior.Cascade);


            base.OnModelCreating(modelBuilder);
        }
    }
}
