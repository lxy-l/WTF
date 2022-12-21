using Consul;
using Consul.AspNetCore;

namespace AuthService.Extensions;

/// <summary>
/// Consul网关配置
/// </summary>
public static class ConsulConfig
{
    public static IServiceCollection AddConsulConfig(this IServiceCollection Services, IConfiguration Configuration)
    {

        var config = Configuration.GetSection("Consul");
        Services.AddConsul(options =>
        {
            options.Address = new Uri(config["Address"]);
            options.WaitTime = TimeSpan.FromSeconds(
                config["WaitTime"] is not null
                    ? double.Parse(config["WaitTime"])
                    : 30);
        });
        var service = config.GetSection("Service");
        Services.AddConsulServiceRegistration(options =>
        {
            options.ID = service["ID"];
            options.Name = service["Name"];
            options.Port = service["Port"] is not null
                ? int.Parse(service["Port"])
                : throw new Exception("未服务配置端口！");
            options.Address = service["Address"];
            options.Check = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = service["DeregisterCriticalServiceAfter"] is not null
                    ? TimeSpan.FromSeconds(
                        double.Parse(
                            service["DeregisterCriticalServiceAfter"]))
                    : TimeSpan.FromSeconds(20),

                HTTP = service["HTTP"],
                Timeout = service["Timeout"] is not null
                    ? TimeSpan.FromSeconds(
                        double.Parse(
                            service["Timeout"]))
                    : TimeSpan.FromSeconds(20),

                Interval = service["Interval"] is not null
                    ? TimeSpan.FromSeconds(
                        double.Parse(
                            service["Interval"]))
                    : TimeSpan.FromSeconds(20)
            };
        });
        return Services;
    }
}