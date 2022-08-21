using consumer;
using consumer.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));
        services.AddHostedService<Worker>();

        // Health check services. A custom health check service is added for demo.
        services.AddHealthChecks().AddCheck<HealthCheck>("health-check");
        services.AddHostedService<TcpHealthProbeService>();
      
    })
    .Build();

await host.RunAsync();
