using System;

namespace FiddlerBackend.Contracts;

public class MoveComposerCollectionRequestDTO : ConcurrencyTokenAwareDTO
{
	public Guid RequestId { get; set; }

	public Guid ComposerCollectionSourceId { get; set; }

	public Guid ComposerCollectionTargetId { get; set; }

	public Guid? TargetFolderId { get; set; }
}
