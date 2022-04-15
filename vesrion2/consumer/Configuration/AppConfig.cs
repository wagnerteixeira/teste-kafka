namespace consumer.Configuration;
public record AppConfig
{
	public string Topic { get; set; }
	public string Server { get; set; }
	public string ConsumerGroup {get; set;}
}