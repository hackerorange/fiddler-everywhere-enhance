using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class ComposerCollectionMultipleFoldersAndRequestsCreateBodyDTO
{
	public ComposerCollectionFolderDTO FolderBeingMoved { get; set; }

	public IEnumerable<CreateComposerCollectionRequestDTO> Requests { get; set; }

	public IEnumerable<ComposerCollectionFolderDTO> Folders { get; set; }
}
