using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class UpdateComposerCollectionDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromBody]
	public UpdateComposerCollectionBodyDTO UpdateComposerCollectionBodyDTO { get; set; }
}
