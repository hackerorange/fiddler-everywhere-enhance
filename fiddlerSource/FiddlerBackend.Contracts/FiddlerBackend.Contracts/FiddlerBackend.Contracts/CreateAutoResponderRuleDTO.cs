using System;

namespace FiddlerBackend.Contracts;

public class CreateAutoResponderRuleDTO
{
	public string Match { get; set; }

	public string Action { get; set; }

	public string Comment { get; set; }

	public Guid? HeadersFileId { get; set; }

	public Guid? BodyFileId { get; set; }
}
