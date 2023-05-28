using System;

namespace FiddlerBackend.Contracts;

public class InactiveAccountErrorDTO : ErrorDTO
{
	public Guid AccountId { get; set; }
}
