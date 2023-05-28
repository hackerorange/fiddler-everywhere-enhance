using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class DeleteComposerCollectionPublicSharingDTO
{
	[FromRoute]
	public Guid ComposerCollectionId { get; set; }

	[FromHeader(Name = "X-Sharing-Token")]
	public string PublicSharingToken { get; set; }
}
