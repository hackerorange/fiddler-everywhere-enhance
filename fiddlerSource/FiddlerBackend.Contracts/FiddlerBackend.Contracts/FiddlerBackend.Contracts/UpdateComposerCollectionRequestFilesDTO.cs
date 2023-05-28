using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class UpdateComposerCollectionRequestFilesDTO
{
	[FromRoute]
	public Guid ComposerCollectionId { get; set; }

	[FromRoute]
	public Guid Id { get; set; }

	[FromBody]
	public UpdateComposerCollectionRequestFilesBodyDTO UpdateComposerCollectionRequestFilesBodyDTO { get; set; }
}
