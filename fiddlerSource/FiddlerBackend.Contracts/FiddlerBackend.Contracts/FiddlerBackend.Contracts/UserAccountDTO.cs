namespace FiddlerBackend.Contracts;

public class UserAccountDTO : AccountDTO
{
	public Roles FilesRoles { get; set; }

	public Roles SnapshotsRoles { get; set; }

	public Roles RuleSetsRoles { get; set; }

	public Roles ComposerCollectionsRoles { get; set; }

	public Roles JAMSessionsRoles { get; set; }

	public bool IsOwner { get; set; }
}
