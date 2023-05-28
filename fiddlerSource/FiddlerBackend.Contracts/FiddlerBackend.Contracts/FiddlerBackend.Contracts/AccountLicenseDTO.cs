using System;

namespace FiddlerBackend.Contracts;

public class AccountLicenseDTO
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
}
