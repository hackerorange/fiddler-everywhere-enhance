namespace FiddlerBackend.Contracts;

public class SearchUserEventsDTO : IPageable, ISortable
{
	public EventType? Type { get; set; }

	public FiddlerProduct? Product { get; set; }

	public UserSearchTypes UserIs { get; set; } = UserSearchTypes.SenderOrReceiver;


	public bool IncludeRead { get; set; }

	public string OrderBy { get; set; } = "CreatedAt";


	public bool OrderDesc { get; set; } = true;


	public uint Skip { get; set; }

	public uint Take { get; set; } = 10u;

}
