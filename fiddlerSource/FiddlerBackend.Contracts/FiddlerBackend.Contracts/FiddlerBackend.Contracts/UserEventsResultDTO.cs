using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class UserEventsResultDTO
{
	public int TotalCount { get; set; }

	public int TotalUnreadCount { get; set; }

	public IList<EventDTO> Events { get; set; }
}
