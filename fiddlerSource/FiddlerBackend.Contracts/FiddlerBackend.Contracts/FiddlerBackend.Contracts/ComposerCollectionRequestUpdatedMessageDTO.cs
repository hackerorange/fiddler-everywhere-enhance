namespace FiddlerBackend.Contracts;

public class ComposerCollectionRequestUpdatedMessageDTO : ComposerCollectionRequestMessageDTO
{
	public string OldName { get; set; }

	public string OldParameters { get; set; }

	public string OldHttpMethod { get; set; }

	public string OldHttpVersion { get; set; }

	public string OldUrl { get; set; }
}
