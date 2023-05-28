using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class GetComposerCollectionRequestDTO
{
	[FromRoute]
	public Guid ComposerCollectionId { get; set; }

	[FromRoute]
	public Guid Id { get; set; }
}
