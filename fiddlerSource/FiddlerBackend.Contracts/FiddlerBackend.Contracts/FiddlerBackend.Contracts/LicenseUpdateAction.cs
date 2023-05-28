namespace FiddlerBackend.Contracts;

public enum LicenseUpdateAction
{
	Created,
	SubscribersAdded,
	SubscribersRemoved,
	SeatAdded,
	SeatRemoved,
	SeatCancelled,
	SeatResumed,
	SeatMappingUpdated,
	PlanChanged,
	Renewed,
	Cancelled,
	Resumed,
	Relation,
	Transfer,
	Migration,
	Ended
}
