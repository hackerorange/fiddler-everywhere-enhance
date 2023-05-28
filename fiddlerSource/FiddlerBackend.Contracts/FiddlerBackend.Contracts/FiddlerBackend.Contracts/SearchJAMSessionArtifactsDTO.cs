using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class SearchJAMSessionArtifactsDTO : IPageable, ISortable
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromQuery]
	public IList<string> Types { get; set; }

	[FromQuery]
	public string OrderBy { get; set; } = "InternalFiddlerId";


	[FromQuery]
	public bool OrderDesc { get; set; }

	[FromQuery]
	public uint Skip { get; set; }

	[FromQuery]
	public uint Take { get; set; } = 20u;


	[FromQuery]
	public string Filter { get; set; }

	[FromQuery]
	public int? TabId { get; set; }

	[FromQuery]
	public DateTime? StartTime { get; set; }

	[FromQuery]
	public DateTime? EndTime { get; set; }

	[FromUrlEncodedHeader(Name = "X-Auth-Pass")]
	public string Password { get; set; }

	[FromHeader(Name = "X-Sharing-Token")]
	public string SharingToken { get; set; }

	[FromHeader(Name = "X-Public-Sharing-Token")]
	public string PublicSharingToken { get; set; }
}
