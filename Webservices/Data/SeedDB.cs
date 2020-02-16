using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webservices.Data
{
    public class SeedDB
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<RepositoryContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            context.Database.EnsureCreated();
            if (!context.Users.Any())
            {
                ApplicationUser userFeri = new ApplicationUser()
                {
                    Email = "Feri@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "Feri"
                };

                ApplicationUser userAlex = new ApplicationUser()
                {
                    Email = "Alex@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "alex"
                };

                ApplicationUser userAdmin = new ApplicationUser()
                {
                    Email = "Admin@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "admin"
                };

                var t1  = userManager.CreateAsync(userFeri, "1234@qweR").Result;
                var t2  =userManager.CreateAsync(userAlex, "1234@qweR").Result;
                var t3  =userManager.CreateAsync(userAdmin, "1234@qweR").Result;
                
                var t4  =roleManager.CreateAsync(new IdentityRole("Admin")).Result;
                var t5  =roleManager.CreateAsync(new IdentityRole("Editor")).Result;
                var t6  =roleManager.CreateAsync(new IdentityRole("Client")).Result;
                
                var t7  =userManager.AddToRoleAsync(userFeri, "Client").Result;
                
                var t8  =userManager.AddToRoleAsync(userAlex, "Client").Result;
                var t9  =userManager.AddToRoleAsync(userAlex, "Editor").Result;
                
                var t10 = userManager.AddToRoleAsync(userAdmin, "Admin").Result;
                context.SaveChanges();
            }
        }
    }
}
