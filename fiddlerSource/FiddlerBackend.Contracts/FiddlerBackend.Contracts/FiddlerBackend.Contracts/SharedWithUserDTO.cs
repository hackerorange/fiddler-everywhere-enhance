namespace FiddlerBackend.Contracts;

public class SharedWithUserDTO
{
	public string Email { get; set; }

	public BaseUserDTO User { get; set; }

	public int JAMSessionsRoles { get; set; }
}
