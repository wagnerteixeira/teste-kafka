namespace consumer.Configuration;
public record AppConfig
{
	public string Topic { get; set; }
	public string Server { get; set; }
	public string ConsumerGroup {get; set;}
	public bool UseSsl { get; set; } = false;
	public string SslCaLocation { get; set; }
	public string SslCertificateLocation { get; set; }
	public string SslKeyLocation { get; set; }
	public int? HealthCheckTcpPort { get; set; }
}