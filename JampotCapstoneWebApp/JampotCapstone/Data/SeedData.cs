using JampotCapstone.Models;
using Microsoft.AspNetCore.Identity;
using File = JampotCapstone.Models.File;

namespace JampotCapstone.Data;

public class SeedData
{
    public static void Seed(ApplicationDbContext ctx, IServiceProvider provider)
    {
        var userManager = provider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        
        const string password = "Secret!123";
        const string role = "Admin";
        
        if (roleManager.FindByNameAsync("Admin").Result == null)
        {
            roleManager.CreateAsync(new IdentityRole(role)).Wait();
        }

        AppUser admin = new AppUser
        {
            UserName = "admin@example.com",
            Name = "admin",
            SignUpDate = DateTime.UtcNow,
            EmailConfirmed = true
        };
        bool isSuccess = userManager.CreateAsync(admin, password).Result.Succeeded;
        if (isSuccess)
        {
            userManager.AddToRoleAsync(admin, role).Wait();
        }

        if (!ctx.Files.Any())
        {
            File landing1 = new File
            {
                FileID = 1,
                FileName = "/pics/JampotDrink.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(landing1);
            
            File landing2 = new File
            {
                FileID = 2,
                FileName = "/pics/jackfruit-wrap.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(landing2);
            
            File landing3 = new File
            {
                FileID = 3,
                FileName = "/pics/ReggaeChiaPudding.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(landing3);
            
            File landing4 = new File
            {
                FileID = 4,
                FileName = "/pics/map.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(landing4);

            File landing5 = new File
            {
                FileID = 5,
                FileName = "/pics/special.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(landing5);
            
            File landing6 = new File
            {
                FileID = 6,
                FileName = "/pics/people.png",
                ContentType = "image/png"
            };
            ctx.Files.Add(landing6);
            ctx.SaveChanges();
        }
    }
}