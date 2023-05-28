using System;

namespace FiddlerBackend.Contracts;

public class AddSnapshotVersionDTO
{
	public Guid FileId { get; set; }

	public bool IsDelta { get; set; }

	public bool IsPasswordProtected { get; set; }

	public bool IsPasswordUpdated { get; set; }
}
