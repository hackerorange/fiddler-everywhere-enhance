namespace FiddlerBackend.Contracts;

public class UpdateUserDTO
{
	public string Name { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string Username { get; set; }

	public bool? SeenGettingStartedInJam { get; set; }

	public string AcceptedJamEULAVersion { get; set; }

	public string AcceptedEverywhereEULAVersion { get; set; }

	public bool? DisabledAnalyticsTracking { get; set; }
}
