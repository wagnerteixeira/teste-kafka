namespace consumer.Dto;

public record Payload
{
	public int Integer { get; set; }
	public string String { get; set; }
	public double Double { get; set; }
	public DateTime DataTime { get; set; }
}
