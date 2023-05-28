using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class CompleteMultipartUploadRequestDTO
{
	[FromRoute]
	public Guid FileId { get; set; }

	[FromBody]
	public FileUploadDTO FileUpload { get; set; } = new FileUploadDTO();

}
