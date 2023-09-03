using Microsoft.EntityFrameworkCore;
using PetShopAPIV2.Entities;

namespace PetShopAPIV2.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Pet> Pets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey((userRole) => new { userRole.UserId, userRole.RoleId });
            modelBuilder.Entity<UserRole>()
                .HasOne((userRole) => userRole.User)
                .WithMany((role) => role.UserRoles)
                .HasForeignKey((userRole) => userRole.UserId);
            modelBuilder.Entity<UserRole>()
                .HasOne((userRole) => userRole.Role)
                .WithMany((role) => role.UserRoles)
                .HasForeignKey((userRole) => userRole.RoleId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
