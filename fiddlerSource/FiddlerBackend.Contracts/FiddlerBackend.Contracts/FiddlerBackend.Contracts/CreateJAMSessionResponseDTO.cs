using System;

namespace FiddlerBackend.Contracts;

public class CreateJAMSessionResponseDTO
{
	public Guid Id { get; set; }

	public string SharingUrl { get; set; }
}
