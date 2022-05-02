using System.Text.Json.Serialization;

namespace producer.Dto;

public record Payload
{
	public int Integer { get; set; }
	public string String { get; set; }
	public double Double { get; set; }
	public DateTime DataTime { get; set; }
	[JsonExtensionData]
	public IDictionary<string, object> Properties { get; set; }
}
