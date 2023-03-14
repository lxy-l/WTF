using Crafty.Application.Core;
using Crafty.Application.Core.ApplicationServices;
using Crafty.Domain.Core.UnitOfWork;
using Crafty.Infrastructure.EFCore.Repository;
using Crafty.Infrastructure.EFCore.UnitOfWork;

using Scrutor;

namespace WebApi.Extensions;

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

        Services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

        Services.AddTransient(typeof(IEfCoreRepository<,>), typeof(EfCoreRepository<,>));

        Services.AddTransient(typeof(IBaseService<,>), typeof(BaseService<,>));
        Services.AddTransient(typeof(IBaseService<,,>), typeof(BaseService<,,>));

        Services.Scan(scan => scan
            .FromAssembliesOf(typeof(Application.Register), typeof(Register))
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
