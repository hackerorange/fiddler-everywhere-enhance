namespace FiddlerBackend.Contracts;

public interface ISortable
{
	string OrderBy { get; set; }

	bool OrderDesc { get; set; }
}
