using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class SearchJAMWorkspaceSessionsDTO : IPageable, ISortable
{
	[FromRoute]
	public Guid Id { get; set; }

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
