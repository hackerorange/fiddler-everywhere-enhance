using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class PatchSnapshotDTO : ConcurrencyTokenAwareDTO
{
	[FromRoute]
	public Guid SnapshotId { get; set; }

	[FromBody]
	public UpdateSnapshotDTO UpdateSnapshot { get; set; }
}
