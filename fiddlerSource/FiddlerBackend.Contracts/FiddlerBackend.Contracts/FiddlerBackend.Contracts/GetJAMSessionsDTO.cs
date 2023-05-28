using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class GetJAMSessionsDTO : IPageable, ISortable
{
	[FromHeader(Name = "X-Upload-Token")]
	public string UploadToken { get; set; }

	[FromQuery]
	public string OrderBy { get; set; } = "CreatedAt";


	[FromQuery]
	public bool OrderDesc { get; set; }

	[FromQuery]
	public uint Skip { get; set; }

	[FromQuery]
	public uint Take { get; set; } = 20u;


	[FromQuery]
	public string Filter { get; set; }
}
