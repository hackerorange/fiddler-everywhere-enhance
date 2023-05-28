namespace FiddlerBackend.Contracts;

public class ComposerCollectionFolderMovedMessageDTO : ComposerCollectionResourceMovedMessageDTO
{
	public ComposerCollectionFolderDTO Folder { get; set; }

	public bool IsEmpty { get; set; }
}
