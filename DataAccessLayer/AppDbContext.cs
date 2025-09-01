using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;
using System.Security.Principal;

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

        public AppDbContext()
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
        private void applyAuditable(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IAuditable).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(typeof(string), Auditable.CreatedBy)
                        .HasMaxLength(20);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(typeof(DateTime), Auditable.CreatedOn);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(typeof(string), Auditable.UpdatedBy);
                    
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(typeof(DateTime), Auditable.UpdatedOn);

                }
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

            applyAuditable(modelBuilder);

            modelBuilder.UseHiLo();

            base.OnModelCreating(modelBuilder);
        }

        private void saveAuditingData()
        {
            foreach (var entry in ChangeTracker.Entries().Where(
                e => e.Entity is IAuditable &&
                (e.State == EntityState.Modified || e.State == EntityState.Added)))
            {
                entry.Property(Auditable.UpdatedBy).CurrentValue = WindowsIdentity.GetCurrent().Name;
                entry.Property(Auditable.UpdatedOn).CurrentValue = DateTime.Now;

                if (entry.State == EntityState.Added)
                {
                    entry.Property(Auditable.CreatedBy).CurrentValue = WindowsIdentity.GetCurrent().Name;
                    entry.Property(Auditable.CreatedOn).CurrentValue = DateTime.Now;
                }
            }
        }

        public override int SaveChanges()
        {
            saveAuditingData();
            Console.WriteLine("old value: " + ChangeTracker.Entries<User>().First().Property(u=>u.id).CurrentValue);
            base.SaveChanges();

            Console.WriteLine("new Value: " + ChangeTracker.Entries<User>().First().Property(u => u.id).CurrentValue);
            return 0;
        }

    }
}
