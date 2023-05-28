using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class UpdateComposerCollectionSharesDTO : ConcurrencyTokenAwareDTO
{
	[FromRoute]
	public Guid ComposerCollectionId { get; set; }

	[FromBody]
	public List<ShareDTO> Shares { get; set; } = new List<ShareDTO>();

}
