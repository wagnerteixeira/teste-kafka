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
	public async Task<IActionResult> Post([FromServices] IOptions<AppConfig> appConfig, [FromBody] Payload payload)
	{
        var msg = JsonSerializer.Serialize(payload);
		var (message, success) = await SendMessageByKafka(msg, appConfig.Value.Server, appConfig.Value.Topic);
		if (success)
		{
			return Created("", message);
		}
		else
		{
			return BadRequest(message);
		}
	}

	private async Task<(string, bool)> SendMessageByKafka(string message, string server, string topic)
	{
		var config = new ProducerConfig { BootstrapServers = server };

		using (var producer = new ProducerBuilder<Null, string>(config).Build())
		{
			try
			{
				var sendResult = await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });

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
