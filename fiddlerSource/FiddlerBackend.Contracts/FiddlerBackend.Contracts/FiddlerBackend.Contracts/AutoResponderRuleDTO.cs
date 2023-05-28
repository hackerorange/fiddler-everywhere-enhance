using System;

namespace FiddlerBackend.Contracts;

public class AutoResponderRuleDTO
{
	public Guid Id { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public string Match { get; set; }

	public string Action { get; set; }

	public string Comment { get; set; }

	public FileDTO Headers { get; set; }

	public FileDTO Body { get; set; }
}
