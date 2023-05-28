namespace FiddlerBackend.Contracts;

public class QuotaDTO
{
	public string Type { get; set; }

	public long Max { get; set; }

	public bool ManuallyAdjusted { get; set; }
}
