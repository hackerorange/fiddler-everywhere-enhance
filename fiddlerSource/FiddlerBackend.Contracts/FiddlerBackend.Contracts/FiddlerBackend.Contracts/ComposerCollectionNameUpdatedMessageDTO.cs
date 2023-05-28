namespace FiddlerBackend.Contracts;

public class ComposerCollectionNameUpdatedMessageDTO : ComposerCollectionNotificationMessageDTO
{
	public string OldName { get; set; }
}
