namespace FiddlerBackend.Contracts;

public class CreateJAMSessionVideoMediaClusterDTO : CreateJAMSessionVideoInitClusterDTO
{
	public decimal? Timecode { get; set; }
}
