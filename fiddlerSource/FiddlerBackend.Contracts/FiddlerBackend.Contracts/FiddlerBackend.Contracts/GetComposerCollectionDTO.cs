using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class GetComposerCollectionDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromHeader(Name = "X-Sharing-Token")]
	public string PublicSharingToken { get; set; }
}
