using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class UpdateQuotasDTO
{
	public string UserEmail { get; set; }

	public IList<QuotaDTO> Quotas { get; set; } = new List<QuotaDTO>();

}
