using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class DeleteSnapshotPublicSharingDTO
{
	[FromRoute]
	public Guid SnapshotId { get; set; }

	[FromHeader(Name = "X-Sharing-Token")]
	public string PublicSharingToken { get; set; }
}
