using consumer;
using consumer.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));

        // Health check services. A custom health check service is added for demo.
        services.AddHealthChecks().AddCheck<HealthCheck>("health-check");
        services.AddHostedService<TcpHealthProbeService>();


        services.AddHostedService<Worker>();       
      
    })
    .Build();

await host.RunAsync();
