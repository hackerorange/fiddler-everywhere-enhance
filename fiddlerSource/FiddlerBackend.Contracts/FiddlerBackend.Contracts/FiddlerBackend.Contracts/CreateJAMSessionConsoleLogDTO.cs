namespace FiddlerBackend.Contracts;

[ArtifactType("console_log")]
public class CreateJAMSessionConsoleLogDTO : CreateJAMSessionArtifactDTO
{
	public override string Type { get; set; } = "console_log";


	public string Message { get; set; }

	public string Level { get; set; }

	public string Stacktrace { get; set; }
}
