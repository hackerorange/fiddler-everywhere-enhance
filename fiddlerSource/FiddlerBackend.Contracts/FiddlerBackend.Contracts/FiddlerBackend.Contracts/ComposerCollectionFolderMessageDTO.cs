namespace FiddlerBackend.Contracts;

public class ComposerCollectionFolderMessageDTO : ComposerCollectionNotificationMessageDTO
{
	public ComposerCollectionFolderDTO Folder { get; set; }

	public bool IsEmpty { get; set; }
}
