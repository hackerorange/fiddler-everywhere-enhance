using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class NewSnapshotVersionDTO : ConcurrencyTokenAwareDTO
{
	[FromRoute]
	public Guid SnapshotId { get; set; }

	[FromBody]
	public AddSnapshotVersionDTO Version { get; set; }
}
