using EvaluateSensorLog.Data.Interfaces;
using EvaluateSensorLog.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EvaluateSensorLog.Data
{
    /// <summary>
    /// Provides methods for adding EvaluateSensorLog.Data to the dependency injection service collection.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds dependency injection for the EvaluateSensorLog.Data project to the services collection.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the EvaluateSensorLog.Data to.</param>
        /// <returns>The Microsoft.Extensions.DependencyInjection.IServiceCollection so that additional calls can be chained.</returns>
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            services
                .AddTransient<IParseSensorRecordRepository, ParseSensorRecordRepository>()
                .AddTransient<IValidateSensorRecordRepository, ValidateSensorRecordRepository>();

            return services;
        }
    }
}
