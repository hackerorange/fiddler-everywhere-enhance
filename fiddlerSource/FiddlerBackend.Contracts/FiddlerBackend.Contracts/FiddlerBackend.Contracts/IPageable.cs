namespace FiddlerBackend.Contracts;

public interface IPageable
{
	uint Skip { get; set; }

	uint Take { get; set; }
}
