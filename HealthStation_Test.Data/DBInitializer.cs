using HealthStation_Test.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthStation_Test.Data
{
   public class DBInitializer : CreateDatabaseIfNotExists<MyContext>
    {
        protected override void Seed(MyContext context)
        {
            SeedRoles(context);
            SeedUsers(context);

            //SeedCategories(context);
            //SeedConfigurations(context);
        }

        public void SeedRoles(MyContext context)
        {
            List<IdentityRole> rolesInDealDouble = new List<IdentityRole>();

            rolesInDealDouble.Add(new IdentityRole() { Name = "Administrator" });
            rolesInDealDouble.Add(new IdentityRole() { Name = "Staff" });
            rolesInDealDouble.Add(new IdentityRole() { Name = "User" });

            var rolesStore = new RoleStore<IdentityRole>(context);
            var rolesManager = new RoleManager<IdentityRole>(rolesStore);

            foreach (IdentityRole role in rolesInDealDouble)
            {
                if (!rolesManager.RoleExists(role.Name))
                {
                    var result = rolesManager.Create(role);

                    if (result.Succeeded) continue;
                }
            }
        }
        public void SeedUsers(MyContext context)
        {
            var usersStore = new UserStore<LibUser>(context);
            var usersManager = new UserManager<LibUser>(usersStore);

            LibUser admin = new LibUser();
            admin.FullName = "Chukwuka Ugwu";
            admin.Email = "admin@email.com";
            admin.UserName = "admin";
            admin.PhoneNumber = "(312)555-0690";
            admin.NIN = "BC6788899";
            var password = "admin123";

            if (usersManager.FindByEmail(admin.Email) == null)
            {
                var result = usersManager.Create(admin, password);

                if (result.Succeeded)
                {
                    //add necessary roles to admin
                    usersManager.AddToRole(admin.Id, "Administrator");
                    //usersManager.AddToRole(admin.Id, "Staff");
                    //usersManager.AddToRole(admin.Id, "User");
                }
            }
        }

       
    }
}
