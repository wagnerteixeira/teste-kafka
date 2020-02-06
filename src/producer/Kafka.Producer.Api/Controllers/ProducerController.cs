using System;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace Kafka.Producer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromQuery]string fila, [FromQuery] string msg)
        {
            return Created("", SendMessageByKafka(fila, msg));
        }

        private string SendMessageByKafka(string fila, string message)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            var _fila = string.IsNullOrWhiteSpace(fila) ? "fila_pedido" : fila;

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var sendResult = producer
                                        .ProduceAsync(_fila, new Message<Null, string> { Value = message })
                                            .GetAwaiter()
                                                .GetResult();

                    return $"Mensagem '{sendResult.Value}' de '{sendResult.TopicPartitionOffset}'";
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }

            return string.Empty;
        }

    }
}