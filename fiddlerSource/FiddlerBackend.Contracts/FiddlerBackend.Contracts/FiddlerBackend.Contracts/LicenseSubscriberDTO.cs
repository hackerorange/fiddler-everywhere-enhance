using System;
using System.ComponentModel.DataAnnotations;

namespace FiddlerBackend.Contracts;

public class LicenseSubscriberDTO : IEquatable<LicenseSubscriberDTO>
{
	[EmailAddress(ErrorMessage = "Invalid subscriber email address specified")]
	public string Email { get; set; }

	public SubscriberRolesDTO Permissions { get; set; }

	bool IEquatable<LicenseSubscriberDTO>.Equals(LicenseSubscriberDTO other)
	{
		return string.Equals(Email, other.Email, StringComparison.InvariantCultureIgnoreCase);
	}

	public override int GetHashCode()
	{
		return Email.GetHashCode();
	}
}
