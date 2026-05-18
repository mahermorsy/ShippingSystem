using DataAccessLayer.Data;
using DataAccessLayer.Identity;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Services
{
    public class ContexConfig
    {
        private  static readonly string SeedEmail = "Admin@Admin.com";
        private static readonly string ReviwerEmail = "Reviwer@Admin.com";
        private static readonly string OperationEmail = "Operation@Admin.com";
        private static readonly string OperationManagerEmail = "OperationManager@Admin.com";
        public static async Task SeedDataAsync(ShippingContext appContext , 
            UserManager<ApplicationUser> userManager ,RoleManager<IdentityRole<Guid>> roleManager ) 
        {
            await SeedUSerAsync(userManager, roleManager);
        }
        public static async Task SeedUSerAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
    

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Reviewer"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Reviewer"));
            }
            if (!await roleManager.RoleExistsAsync("Operation"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Operation"));
            }
            if (!await roleManager.RoleExistsAsync("Operation Manager"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Operation Manager"));
            }

                if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("User"));
            }

            var SeedAminUSer  = await userManager.FindByEmailAsync(SeedEmail);
            if (SeedAminUSer == null)
            {
                SeedAminUSer = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "Admin",
                    Email = SeedEmail,
                    EmailConfirmed = true,
                };

                var SeedResult = await userManager.CreateAsync(SeedAminUSer,"Admin@123");
                await userManager.AddToRoleAsync(SeedAminUSer, "Admin");
            }

            var ReviwerUser = await userManager.FindByEmailAsync(ReviwerEmail);
            if (ReviwerUser == null)
            {
                ReviwerUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "ReviwerUser",
                    Email = ReviwerEmail,
                    EmailConfirmed = true,
                };
                var SeedResult = await userManager.CreateAsync(ReviwerUser, "Reviwer@123");
                await userManager.AddToRoleAsync(ReviwerUser, "Reviewer");
            }


            var Operationuser = await userManager.FindByEmailAsync(OperationEmail);
            if (Operationuser == null)
            {
                Operationuser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "Operationuser",
                    Email = OperationEmail,
                    EmailConfirmed = true,
                };

                var SeedResult = await userManager.CreateAsync(Operationuser, "Operation@123");
                await userManager.AddToRoleAsync(Operationuser, "Operation");
            }

            var OperationManagerUser = await userManager.FindByEmailAsync(OperationManagerEmail);
            if (OperationManagerUser == null)
            {
                OperationManagerUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "OperationManagerUser",
                    Email = OperationManagerEmail,
                    EmailConfirmed = true,
                };

                var SeedResult = await userManager.CreateAsync(OperationManagerUser, "OperationManager@123");
                await userManager.AddToRoleAsync(OperationManagerUser, "Operation Manager");
            }

        }

    }
}
