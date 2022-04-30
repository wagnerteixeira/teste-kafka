using System.Text.Json;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using producer.Configuration;
using producer.Dto;

namespace producer.Controllers;



[Route("api/[controller]")]
[ApiController]
public class ProducerController : ControllerBase
{
	[HttpPost]
	[ProducesResponseType(typeof(string), 201)]
	[ProducesResponseType(400)]
	[ProducesResponseType(500)]
	public async Task<IActionResult> Post([FromServices] IOptions<AppConfig> appConfig, [FromServices] ILogger<ProducerController> logger, [FromBody] Payload payload)
	{
		JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
		{
		};
        var msg = JsonSerializer.Serialize(payload, options);
		logger.LogInformation("Send message {message}", msg);
		var (message, success) = await SendMessageByKafka(msg, appConfig.Value);
		if (success)
		{
			return Created("", message);
		}
		else
		{
			return BadRequest(message);
		}
	}

	private async Task<(string, bool)> SendMessageByKafka(string message, AppConfig appConfig)
	{
		var config = new ProducerConfig { 
			BootstrapServers = appConfig.Server,
		};

		if (appConfig.UseSsl)
		{
			config = new ProducerConfig(config)
			{
				SecurityProtocol = SecurityProtocol.Ssl,
				SslCaLocation = appConfig.SslCaLocation,
				SslCertificateLocation = appConfig.SslCertificateLocation,
				SslKeyLocation = appConfig.SslKeyLocation,
				Debug = "security"
			};
		}
	
		using (var producer = new ProducerBuilder<Null, string>(config).Build())
		{
			try
			{
				var sendResult = await producer.ProduceAsync(appConfig.Topic, new Message<Null, string> { Value = message });

				return ($"Mensagem '{sendResult.Value}' de '{sendResult.TopicPartitionOffset}'", true);
			}
			catch (ProduceException<Null, string> e)
			{
				Console.WriteLine($"Delivery failed: {e.Error.Reason}");
				return (e.Error.Reason, false);
			}
		}
	}

}
