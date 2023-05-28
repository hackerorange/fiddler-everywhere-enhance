namespace FiddlerBackend.Contracts;

public class JAMSessionEmailDTO : NotificationMessageDTO
{
	public override FiddlerProduct Product => FiddlerProduct.Jam;

	public string Domain { get; set; }

	public string PortalShareUrl { get; set; }

	public string Description { get; set; }
}
