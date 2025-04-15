using IBusinessLogic;
using BusinessLogic;
using DataAccess.Repositories;
using DataAccess.Context;
using IDataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace APIServiceFactory
{
    public static class ServiceFactory
    {
        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserLogic, UserLogic>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddScoped<IHomeLogic, HomeLogic>();
            serviceCollection.AddScoped<IHomeRepository, HomeRepository>();
            serviceCollection.AddScoped<ICompanyLogic, CompanyLogic>();
            serviceCollection.AddScoped<ICompanyRepository, CompanyRepository>();
            serviceCollection.AddScoped<IDeviceLogic, DeviceLogic>();
            serviceCollection.AddScoped<IDeviceRepository, DeviceRepository>();
            serviceCollection.AddScoped<ISessionRepository, SessionRepository>();
            serviceCollection.AddScoped<IActionLogic, ActionLogic>();
        }

        public static void AddConnectionString(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<DbContext, SmartHomeContext>(o => o.UseSqlServer(connectionString));
        }

    }
}
