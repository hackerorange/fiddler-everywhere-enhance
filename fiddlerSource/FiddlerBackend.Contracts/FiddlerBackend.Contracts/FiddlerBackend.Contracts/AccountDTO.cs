using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class AccountDTO
{
	public Guid Id { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public string Name { get; set; }

	public FiddlerProduct Product { get; set; }

	public Guid OwnerId { get; set; }

	public string OwnerEmail { get; set; }

	public Guid? LicenseAccountId { get; set; }

	public string LicensePlanName { get; set; }

	public string LicensePlanPeriod { get; set; }

	public bool LicenseIsCancelled { get; set; }

	public PaymentSource LicensePaymentSource { get; set; }

	public DateTime? LicenseCreatedAt { get; set; }

	public DateTime? LicenseExpiresOn { get; set; }

	public DateTime? MigrationStartedOn { get; set; }

	public DateTime? MigratedOn { get; set; }

	public DateTime? LastBillingCycleStart { get; set; }

	public DateTime? NextBillingCycleStart { get; set; }

	public bool IsInactive { get; set; }

	public DateTime? UpgradedFromTrialOn { get; set; }

	public string ApiKey { get; set; }

	public IList<QuotaDTO> Quotas { get; set; } = new List<QuotaDTO>();

}
