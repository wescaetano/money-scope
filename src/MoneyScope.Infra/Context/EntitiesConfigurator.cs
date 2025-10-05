using Microsoft.EntityFrameworkCore;
using MoneyScope.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Infra.Context
{
    public static class EntitiesConfigurator
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new ProfileModuleMap());

        }
    }
}
