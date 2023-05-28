using System;

namespace FiddlerBackend.Contracts;

public class BaseUserDTO
{
	public Guid Id { get; set; }

	public string Email { get; set; }

	public string Name { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string Username { get; set; }

	public bool Incomplete { get; set; }

	public bool Internal { get; set; }

	public bool IsEverywhereTrialUsed { get; set; }

	public string EverywhereTrialMachineId { get; set; }

	public bool IsJamTrialUsed { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }
}
