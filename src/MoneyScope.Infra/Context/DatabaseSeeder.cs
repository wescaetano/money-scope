using Microsoft.EntityFrameworkCore;
using MoneyScope.Core.Enums.Profile;
using MoneyScope.Core.Enums.User;
using MoneyScope.Domain;
using MoneyScope.Domain.AccessProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Infra.Context
{
    public static class DatabaseSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {

            //Funcionalidades
            modelBuilder.Entity<Module>().HasData(
                new Module { Id = 1, Name = "Users", Visualize = true, Edit = true, Register = true, Inactivate = true, Exclude = true }
                );

            modelBuilder.Entity<Profile>().HasData(
                new Profile { Id = 1, Name = "Admin", Status = EProfileStatus.Ativo },
                new Profile { Id = 4, Name = "User", Status = EProfileStatus.Ativo }
            );

            //PerfilFuncionalidade Admin
            modelBuilder.Entity<ProfileModule>().HasData(
                new ProfileModule { ProfileId = 1, ModuleId = 1, Visualize = true, Edit = true, Register = true, Exclude = true, Inactivate = true }
                );

            //PerfilFuncionalidade Marketing
            modelBuilder.Entity<ProfileModule>().HasData(
                new ProfileModule { ProfileId = 8, ModuleId = 1, Visualize = false, Edit = false, Register = false, Exclude = false, Inactivate = false }
                );


            //usuario

            var adminUser = new User
            {
                Name = "Admin",
                Email = "admin@admin.com",
                Status = EUserStatus.Ativo,
                Password = "$2a$15$5DxGdsCvuzHVigXWk8Qr1uvoizMNxrdxz6SypelRVxC7n1D9uHB7."
            };
            adminUser.SetId(1);


            //Usuario
            modelBuilder.Entity<User>().HasData(
               adminUser
            );
        }
    }
}
