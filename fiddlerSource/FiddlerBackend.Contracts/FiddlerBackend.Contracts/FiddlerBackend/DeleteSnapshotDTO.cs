using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend;

public class DeleteSnapshotDTO : ConcurrencyTokenAwareDTO
{
	[FromRoute]
	public Guid SnapshotId { get; set; }
}
