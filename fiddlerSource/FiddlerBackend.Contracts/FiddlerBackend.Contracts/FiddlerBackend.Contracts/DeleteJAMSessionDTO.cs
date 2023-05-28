using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class DeleteJAMSessionDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromHeader(Name = "X-Upload-Token")]
	public string UploadToken { get; set; }
}
