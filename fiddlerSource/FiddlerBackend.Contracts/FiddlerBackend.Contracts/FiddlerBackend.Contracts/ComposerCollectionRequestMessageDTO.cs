namespace FiddlerBackend.Contracts;

public class ComposerCollectionRequestMessageDTO : ComposerCollectionNotificationMessageDTO
{
	public ComposerCollectionRequestDTO Request { get; set; }
}
