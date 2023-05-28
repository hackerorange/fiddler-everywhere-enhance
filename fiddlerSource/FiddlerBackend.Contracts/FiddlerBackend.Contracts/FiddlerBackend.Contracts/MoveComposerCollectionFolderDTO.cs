using System;

namespace FiddlerBackend.Contracts;

public class MoveComposerCollectionFolderDTO : ConcurrencyTokenAwareDTO
{
	public Guid FolderId { get; set; }

	public Guid ComposerCollectionSourceId { get; set; }

	public Guid ComposerCollectionTargetId { get; set; }

	public Guid? TargetFolderId { get; set; }
}
