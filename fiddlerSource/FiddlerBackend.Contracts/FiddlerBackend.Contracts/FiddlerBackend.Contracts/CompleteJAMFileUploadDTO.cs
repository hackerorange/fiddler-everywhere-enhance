using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class CompleteJAMFileUploadDTO
{
	[FromRoute]
	public Guid FileId { get; set; }

	[FromBody]
	public List<PartETagDTO> Parts { get; set; } = new List<PartETagDTO>();

}
