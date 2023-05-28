using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class PatchEventDTO
{
	[FromRoute]
	public Guid EventId { get; set; }

	[FromBody]
	public UpdateEventDTO UpdateEvent { get; set; }
}
