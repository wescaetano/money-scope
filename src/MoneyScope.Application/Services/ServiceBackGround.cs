using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Services
{
    public class ServiceBackGround : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceBackGround(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Executou o agendamento dos bgServices");
            var tz = TimeZoneInfo.FindSystemTimeZoneById(
             OperatingSystem.IsWindows()
                 ? "E. South America Standard Time"
                 : "America/Sao_Paulo"
             );

            var options = new RecurringJobOptions
            {
                TimeZone = tz
            };

            using (var scope = _serviceProvider.CreateScope())
            {
                //var _recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                //_recurringJobManager.AddOrUpdate<IKanbanService>(
                //    "busca-kanbans-protheus",
                //    service => service.BuscaKanbansProtheus(),
                //    Cron.Daily(3),
                //    options
                //);

                //_recurringJobManager.AddOrUpdate<IUsuarioService>(
                //    "busca-usuarios-protheus",
                //    service => service.BuscaUsuariosProtheus(),
                //    Cron.Daily(3, 20),
                //    options
                //);
            }
            return Task.CompletedTask;
        }
    }
}
