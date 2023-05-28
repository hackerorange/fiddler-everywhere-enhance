using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class GetSnapshotDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromQuery]
	public bool IncludeDeleted { get; set; }

	[FromHeader(Name = "X-Sharing-Token")]
	public string PublicSharingToken { get; set; }
}
