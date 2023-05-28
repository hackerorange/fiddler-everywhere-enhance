using System;

namespace FiddlerBackend.Contracts;

public abstract class CreateJAMSessionArtifactDTO
{
	public abstract string Type { get; set; }

	public DateTime? StartedAt { get; set; }

	public int InternalFiddlerId { get; set; }

	public int? PublicFiddlerId { get; set; }

	public long? TabId { get; set; }

	public long? StartOffset { get; set; }

	public long? EndOffset { get; set; }

	public virtual void Validate()
	{
		if (!StartedAt.HasValue)
		{
			throw new ValidationException("StartedAt field is required", "The StartedAt field is required for creating a new JAM session artifact");
		}
	}
}
