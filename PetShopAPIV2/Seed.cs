using PetShopAPIV2.Data;
using PetShopAPIV2.Entities;

namespace PetShopAPIV2
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)
        {
            _context = context;
        }

        public void SeedDataContext()
        {
            if (!_context.Users.Any())
            {
                Role adminRole = new Role()
                {
                    Name = "Admin"
                };
                Role workerRole = new Role()
                {
                    Name = "Worker"
                };
                Role userRole = new Role()
                {
                    Name = "User"
                };
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                User adminUser = new User()
                {
                    Email = "admin@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin", salt)
                };
                User workerUser = new User()
                {
                    Email = "worker@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("worker", salt)
                };
                User user = new User()
                {
                    Email = "user@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("user", salt)

                };
                List<UserRole> userRoles = new List<UserRole>();
                userRoles.Add(new UserRole()
                {
                    Role = adminRole,
                    User = adminUser
                });
                
                userRoles.Add(new UserRole()
                {
                    Role = workerRole,
                    User = adminUser
                });
                
                userRoles.Add(new UserRole()
                {
                    Role = userRole,
                    User = adminUser
                });
                userRoles.Add(new UserRole()
                {
                    Role = workerRole,
                    User = workerUser
                });
                userRoles.Add(new UserRole()
                {
                    Role = userRole,
                    User = workerUser
                });
                userRoles.Add(new UserRole()
                {
                    Role = userRole,
                    User = user
                });
                
                _context.UserRoles.AddRange(userRoles);
                _context.SaveChanges();
            }
        }
    }
}
