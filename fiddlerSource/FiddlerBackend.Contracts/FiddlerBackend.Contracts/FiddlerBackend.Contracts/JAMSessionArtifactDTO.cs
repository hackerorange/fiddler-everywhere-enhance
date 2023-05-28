using System;

namespace FiddlerBackend.Contracts;

public abstract class JAMSessionArtifactDTO
{
	public Guid Id { get; set; }

	public int? PublicFiddlerId { get; set; }

	public int? InternalFiddlerId { get; set; }

	public DateTime StartedAt { get; set; }

	public string Type { get; set; }

	public long? StartOffset { get; set; }

	public long? EndOffset { get; set; }

	public long? TabId { get; set; }
}
