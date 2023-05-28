using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class InitiateJAMFileUploadDTO
{
	[Required]
	[FromHeader(Name = "X-Upload-Content-Length")]
	public long ContentLength { get; set; }

	[Required]
	[FromHeader(Name = "X-Upload-Content-Type")]
	public string ContentType { get; set; }

	[FromHeader(Name = "X-Upload-Content-Encoding")]
	public string ContentEncoding { get; set; }

	[FromHeader(Name = "X-Upload-Content-MD5")]
	public string ContentMD5 { get; set; }

	[FromUrlEncodedHeader(Name = "X-Upload-Password")]
	public string Password { get; set; }

	[FromHeader(Name = "X-Upload-File-Extension")]
	public string Extension { get; set; }
}
