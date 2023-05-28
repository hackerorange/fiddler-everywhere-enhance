using System;

namespace FiddlerBackend.Contracts;

public class UserDiscountDTO
{
	public string Alias { get; set; }

	public DateTime? ExpirationDate { get; set; }

	public string Product { get; set; }

	public string PlanName { get; set; }

	public bool IsForced { get; set; }

	public bool IsBillingDelayed { get; set; }
}
