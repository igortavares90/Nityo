using Microsoft.Extensions.DependencyInjection;
using Nityo.DigitalAccount.Domain.Context.Interfaces;
using Nityo.DigitalAccount.Infrastructure.Data.DataContext;
using Nityo.DigitalAccount.Infrastructure.Data.Repositories;
using Nityo.DigitalAccount.Infrastructure.Uow;
using System;

namespace Nityo.ContaDigital.Configurations
{
    public static class DependencyInjectionSetup
    {
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<IDigitalAccountRepository, DigitalAccountRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DigitalAccountContext>();

        }
    }
}
