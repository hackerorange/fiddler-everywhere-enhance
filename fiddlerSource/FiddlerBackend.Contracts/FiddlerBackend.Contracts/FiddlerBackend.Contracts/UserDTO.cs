using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class UserDTO : BaseUserDTO
{
	public DateTime? EventsLastRead { get; set; }

	public long UnreadEvents { get; set; }

	public bool SeenGettingStartedInJam { get; set; }

	public IList<UserAccountDTO> Accounts { get; set; } = new List<UserAccountDTO>();


	public string AcceptedJamEULAVersion { get; set; }

	public string AcceptedEverywhereEULAVersion { get; set; }

	public bool? DisabledAnalyticsTracking { get; set; }
}
