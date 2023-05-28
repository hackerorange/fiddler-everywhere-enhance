using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class ComposerCollectionMultipleFoldersAndRequestsCreateDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromBody]
	public ComposerCollectionMultipleFoldersAndRequestsCreateBodyDTO ComposerCollectionMultipleFoldersAndRequestsCreateBodyDTO { get; set; }
}
