using Microsoft.Extensions.DependencyInjection;
using MoneyScope.Application.Config;
using MoneyScope.Core.Token;
using MoneyScope.Infra.Interfaces;
using MoneyScope.Infra.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Ioc
{
    public static class Injection
    {
        public static IServiceCollection InjectDependencies(this IServiceCollection services, MigrationConfig? migrationConfig)
        {
            //if (migrationConfig != null && migrationConfig.AplicarMigration == true)
            //{
            //    using (var scope = services.BuildServiceProvider().CreateScope())
            //    {
            //        var dbContext = scope.ServiceProvider.GetRequiredService<SantoAndreContext>();
            //        dbContext.Database.Migrate();
            //    }
            //}


            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IBaseRelationRepository<>), typeof(BaseRelationRepository<>));
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();

            //Services
            //services.AddScoped(typeof(IBaseService), typeof(BaseService));
            //services.AddScoped(typeof(ITokenService), typeof(TokenService));
            //services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(TokenConfigurations));

            return services;
        }
    }
}
