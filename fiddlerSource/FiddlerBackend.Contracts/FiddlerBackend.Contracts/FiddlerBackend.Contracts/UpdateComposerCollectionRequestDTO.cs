using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class UpdateComposerCollectionRequestDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromRoute]
	public Guid ComposerCollectionId { get; set; }

	[FromBody]
	public UpdateComposerCollectionRequestBodyDTO UpdateComposerCollectionRequestBodyDTO { get; set; }
}
