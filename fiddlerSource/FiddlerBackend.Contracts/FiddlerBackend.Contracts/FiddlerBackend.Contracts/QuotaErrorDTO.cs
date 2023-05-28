namespace FiddlerBackend.Contracts;

public class QuotaErrorDTO : ErrorDTO
{
	public string ExceededQuota { get; set; }

	public QuotaValueDTO QuotaUsage { get; set; }

	public long NewValue { get; set; }
}
