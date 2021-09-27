using EvaluateSensorLog.ClassLibrary.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EvaluateSensorLog.ClassLibrary
{
    /// <summary>
    /// Provides methods for adding EvaluateSensorLog.ClassLibrary to the dependency injection service collection.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds dependency injection for the EvaluateSensorLog.ClassLibrary project to the services collection.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the EvaluateSensorLog.ClassLibrary to.</param>
        /// <returns>The Microsoft.Extensions.DependencyInjection.IServiceCollection so that additional calls can be chained.</returns>
        public static IServiceCollection AddClassLibrary(this IServiceCollection services)
        {
            services.AddTransient<IEvaluateSensorLogRecords, EvaluateSensorLogRecords>();

            return services;
        }
    }
}
