using System;
using System.ComponentModel.DataAnnotations;

namespace FiddlerBackend.Contracts;

public class ShareDTO : IEquatable<ShareDTO>
{
	[EmailAddress(ErrorMessage = "Invalid email address specified")]
	public string Email { get; set; }

	public string Note { get; set; }

	public string Reason => Note;

	public bool Equals(ShareDTO other)
	{
		return string.Equals(Email, other.Email, StringComparison.InvariantCultureIgnoreCase);
	}

	public override int GetHashCode()
	{
		return Email.GetHashCode();
	}
}
