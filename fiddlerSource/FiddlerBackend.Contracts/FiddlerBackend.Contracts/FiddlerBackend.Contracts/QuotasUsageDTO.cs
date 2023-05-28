using System;

namespace FiddlerBackend.Contracts;

public class QuotasUsageDTO
{
	public Guid AccountId { get; set; }

	public QuotaValueDTO TotalAccountSize { get; set; }

	public QuotaValueDTO LocalSnapshots { get; set; }

	public QuotaValueDTO LocalAutoResponderRules { get; set; }

	public QuotaValueDTO LocalFilters { get; set; }

	public QuotaValueDTO SharedSnapshots { get; set; }

	public QuotaValueDTO SnapshotSize { get; set; }

	public QuotaValueDTO SnapshotSharedWithUsers { get; set; }

	public QuotaValueDTO SharedRuleSets { get; set; }

	public QuotaValueDTO RuleSetSharedWithUsers { get; set; }

	public QuotaValueDTO SharedComposerRequests { get; set; }

	public QuotaValueDTO ComposerRequestsSharedWithUsers { get; set; }

	public QuotaValueDTO LocalSessionsStatisticsLimit { get; set; }

	public QuotaValueDTO JAMSessionsCount { get; set; }

	public QuotaValueDTO AssignedSeats { get; set; }

	public QuotaValueDTO Viewers { get; set; }
}
