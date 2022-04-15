namespace producer.Configuration;

public record AppConfig
{
	public string Topic { get; set; }
	public string Server { get; set; }
}
