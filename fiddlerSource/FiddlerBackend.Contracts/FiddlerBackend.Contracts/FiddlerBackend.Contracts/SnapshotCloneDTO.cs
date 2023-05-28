using System;

namespace FiddlerBackend.Contracts;

public class SnapshotCloneDTO
{
	public Guid Id { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }
}
