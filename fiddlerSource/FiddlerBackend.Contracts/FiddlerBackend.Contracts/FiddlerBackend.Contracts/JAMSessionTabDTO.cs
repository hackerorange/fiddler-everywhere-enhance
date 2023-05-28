namespace FiddlerBackend.Contracts;

public class JAMSessionTabDTO : JAMSessionArtifactDTO
{
	public string Url { get; set; }

	public string Title { get; set; }

	public bool? IsClosed { get; set; }

	public long? OpenerTabId { get; set; }
}
