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
        public DbSet<Phone> phones { get; set; }
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


            modelBuilder.Entity<User>()
                .HasKey(u => u.id);

            modelBuilder.Entity<Address>()
                .HasKey(a => a.userId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.address)
                .WithOne(a => a.user)
                .HasForeignKey<Address>(a => a.userId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Phone>()
                .HasKey(p => p.Id);

            //no need to explictly set the forign key as the forign is exist in many side 
            modelBuilder.Entity<User>()
                .HasMany(u => u.phones)
                .WithOne(p => p.user)
                .IsRequired();




            base.OnModelCreating(modelBuilder);
        }
    }
}
