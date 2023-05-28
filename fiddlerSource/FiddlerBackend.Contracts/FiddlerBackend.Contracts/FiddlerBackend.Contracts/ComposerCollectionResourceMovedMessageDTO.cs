namespace FiddlerBackend.Contracts;

public abstract class ComposerCollectionResourceMovedMessageDTO : ComposerCollectionNotificationMessageDTO
{
	public ComposerCollectionDTO OldCollection { get; set; }

	public ComposerCollectionDTO NewCollection { get; set; }

	public ComposerCollectionRequestParentDTO OldFolder { get; set; }

	public ComposerCollectionRequestParentDTO NewFolder { get; set; }
}
