using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class FileUploadDTO
{
	public Guid AccountId { get; set; }

	public List<PartETagDTO> Parts { get; set; } = new List<PartETagDTO>();

}
