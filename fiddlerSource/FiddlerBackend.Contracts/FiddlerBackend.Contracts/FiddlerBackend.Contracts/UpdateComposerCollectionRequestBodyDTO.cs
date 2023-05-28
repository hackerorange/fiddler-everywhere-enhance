namespace FiddlerBackend.Contracts;

public class UpdateComposerCollectionRequestBodyDTO : ConcurrencyTokenAwareDTO
{
	public string Name { get; set; }

	public string Description { get; set; }

	public string Parameters { get; set; }

	public string HttpMethod { get; set; }

	public string HttpVersion { get; set; }

	public string Url { get; set; }
}
