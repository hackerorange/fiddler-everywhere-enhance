namespace FiddlerBackend.Contracts;

public class UpdateComposerCollectionBodyDTO : ConcurrencyTokenAwareDTO
{
	public string Name { get; set; }

	public string Description { get; set; }
}
