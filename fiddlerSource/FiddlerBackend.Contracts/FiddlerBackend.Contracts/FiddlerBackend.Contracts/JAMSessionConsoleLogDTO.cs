namespace FiddlerBackend.Contracts;

public class JAMSessionConsoleLogDTO : JAMSessionArtifactDTO
{
	public string Message { get; set; }

	public string Level { get; set; }

	public string Stacktrace { get; set; }
}
