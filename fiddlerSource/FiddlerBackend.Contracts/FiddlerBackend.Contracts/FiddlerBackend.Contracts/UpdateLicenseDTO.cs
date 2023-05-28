using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FiddlerBackend.Contracts;

public class UpdateLicenseDTO
{
	[EmailAddress(ErrorMessage = "Invalid email address specified")]
	public string PurchaserEmail { get; set; }

	public LicenseSubscriptionDTO Subscription { get; set; }

	public IList<LicenseUpdateAction> Actions { get; set; }

	public IList<string> RemovedSubscribersEmails { get; set; }

	public IList<string> AddedSubscribersEmails { get; set; }

	public AccountRelationType? Relation { get; set; }

	public Guid? RelatedAccountId { get; set; }
}
