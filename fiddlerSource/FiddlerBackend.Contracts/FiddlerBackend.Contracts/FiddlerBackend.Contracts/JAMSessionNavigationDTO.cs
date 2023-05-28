namespace FiddlerBackend.Contracts;

public class JAMSessionNavigationDTO : JAMSessionArtifactDTO
{
	public string Url { get; set; }

	public string TransitionType { get; set; }

	public bool? IsHistoryStateUpdated { get; set; }
}
