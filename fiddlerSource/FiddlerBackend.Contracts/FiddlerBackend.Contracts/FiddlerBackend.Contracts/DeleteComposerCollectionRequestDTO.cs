using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class DeleteComposerCollectionRequestDTO : ConcurrencyTokenAwareDTO
{
	[FromRoute]
	public Guid ComposerCollectionId { get; set; }

	[FromRoute]
	public Guid Id { get; set; }
}
