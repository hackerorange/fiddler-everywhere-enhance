using System;

namespace FiddlerBackend.Contracts;

public class UpdateComposerCollectionRequestFilesBodyDTO : ConcurrencyTokenAwareDTO
{
	public Guid? HeadersFileId { get; set; }

	public Guid? BodyFileId { get; set; }
}
