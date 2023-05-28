namespace FiddlerBackend.Contracts;

public class ComposerCollectionFolderNameUpdatedMessageDTO : ComposerCollectionFolderMessageDTO
{
	public string OldName { get; set; }
}
