namespace FiddlerBackend.Contracts;

public class JAMSessionWebsocketDTO : JAMSessionArtifactDTO
{
	public string Url { get; set; }

	public bool? IsClosed { get; set; }
}
