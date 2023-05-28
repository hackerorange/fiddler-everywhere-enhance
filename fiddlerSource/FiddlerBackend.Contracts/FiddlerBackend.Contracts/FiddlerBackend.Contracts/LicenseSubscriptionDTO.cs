using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class LicenseSubscriptionDTO
{
	public Guid Id { get; set; }

	public string Status { get; set; }

	public long SeatCapacity { get; set; } = 1L;


	public DateTime? CreatedAt { get; set; }

	public DateTime? ExpiresOn { get; set; }

	public FiddlerProduct Product { get; set; } = FiddlerProduct.Everywhere;


	public PaymentSource PaymentSource { get; set; }

	public string SubscriptionCategory { get; set; }

	public string BillingCycle { get; set; }

	public IList<LicenseSubscriberDTO> Subscribers { get; set; } = new List<LicenseSubscriberDTO>();

}
