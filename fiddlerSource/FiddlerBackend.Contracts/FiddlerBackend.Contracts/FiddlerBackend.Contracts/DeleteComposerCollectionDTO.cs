using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class DeleteComposerCollectionDTO : ConcurrencyTokenAwareDTO
{
	[FromRoute]
	public Guid Id { get; set; }
}
