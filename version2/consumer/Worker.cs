using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using consumer.Configuration;
using consumer.Dto;

namespace consumer;

public class Worker : BackgroundService
{
	private readonly ILogger<Worker> _logger;
	private readonly AppConfig _appConfig;

	public Worker(ILogger<Worker> logger, IOptions<AppConfig> appConfig)
	{
		_logger = logger;
		_appConfig = appConfig.Value;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var config = new ConsumerConfig
		{
			BootstrapServers = _appConfig.Server,
			AutoOffsetReset = AutoOffsetReset.Earliest,
			GroupId = _appConfig.ConsumerGroup
		};

		if (_appConfig.UseSsl)
		{
			config = new ConsumerConfig(config)
			{
				SecurityProtocol = SecurityProtocol.Ssl,
				SslCaLocation = _appConfig.SslCaLocation,
				SslCertificateLocation = _appConfig.SslCertificateLocation,
				SslKeyLocation = _appConfig.SslKeyLocation,
				Debug = "security"
			};
		}

		try
		{
			using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
			{
				consumerBuilder.Subscribe(_appConfig.Topic);

				try
				{
					while (!stoppingToken.IsCancellationRequested)
					{
						var consumer = consumerBuilder.Consume(stoppingToken);
						_logger.LogInformation($"Receive message {consumer.Message.Value} de {consumer.TopicPartitionOffset}");

						var payload = JsonSerializer.Deserialize<Payload>(consumer.Message.Value);
						await Task.Delay(1000, stoppingToken);
					}
				}
				catch (OperationCanceledException)
				{
					consumerBuilder.Close();
				}
			}
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error when execute");
		}
	}
}
