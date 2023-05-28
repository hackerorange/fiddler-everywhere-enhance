namespace FiddlerBackend.Contracts;

public class SubscriberRolesDTO
{
	public Roles Files { get; set; }

	public Roles Snapshots { get; set; }

	public Roles AutoResponderRuleSets { get; set; }

	public Roles ComposerCollections { get; set; }

	public Roles JAMSessions { get; set; }
}
