using Application.Core.ApplicationServices;
using Domain.Core.Repository;
using Infrastructure.Core.Repository.EFCore;
using Infrastructure.Core.UnitOfWork;
using Scrutor;

namespace WebApi.ServicesConfig;

/// <summary>
/// 注入服务配置
/// </summary>
public static class ServiceConfig
{
    public static void AddServicesConfig(this IServiceCollection Services)
    {
        if (Services == null) throw new ArgumentNullException(nameof(Services));

        #region 服务配置

        //TODO 可以考虑改用AutoFac注入

        Services.AddScoped<IUnitOfWork, UnitOfWork>();

        Services.AddTransient(typeof(IEfCoreRepositoryAsync<,>), typeof(EfCoreRepositoryAsync<,>));

        Services.AddTransient(typeof(IBaseService<,>), typeof(BaseService<,>));

        Services.Scan(scan => scan
            .FromAssembliesOf(typeof(Application.Register), typeof(Application.Core.Register))
            .AddClasses(classes => classes.Where(c => c.Name.EndsWith("Service")))
            .UsingRegistrationStrategy(RegistrationStrategy.Throw)
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        //Services
        //    .RegisterAssemblyPublicNonGenericClasses(Assembly.GetAssembly(typeof(Application.Register)))
        //    .Where(x => x.Name.EndsWith("Service"))
        //    .AsPublicImplementedInterfaces();
        #endregion
    }
}
