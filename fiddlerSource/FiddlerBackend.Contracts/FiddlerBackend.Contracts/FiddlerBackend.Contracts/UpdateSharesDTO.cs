using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class UpdateSharesDTO : ConcurrencyTokenAwareDTO
{
	[FromRoute]
	public Guid SnapshotId { get; set; }

	[FromBody]
	public List<ShareDTO> Shares { get; set; }
}
