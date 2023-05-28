using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class UpdateComposerCollectionFolderDTO
{
	[FromRoute]
	public Guid ComposerCollectionId { get; set; }

	[FromRoute]
	public Guid Id { get; set; }

	[FromBody]
	public UpdateComposerCollectionFolderBodyDTO UpdateComposerCollectionFolderBodyDTO { get; set; }
}
