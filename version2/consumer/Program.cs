using consumer;
using consumer.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));
        services.AddHostedService<Worker>();
      
    })
    .Build();

await host.RunAsync();
