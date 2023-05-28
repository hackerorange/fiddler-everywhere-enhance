namespace FiddlerBackend.Contracts;

public class ComposerCollectionRequestMovedMessageDTO : ComposerCollectionResourceMovedMessageDTO
{
	public ComposerCollectionRequestDTO Request { get; set; }
}
