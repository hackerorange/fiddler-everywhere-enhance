using System;

namespace FiddlerBackend.Contracts;

public class RuleSetShareDTO : ShareDTO, IEquatable<RuleSetShareDTO>
{
	public Roles Permissions { get; set; }

	public bool Equals(RuleSetShareDTO other)
	{
		return Equals((ShareDTO)other);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}
}
