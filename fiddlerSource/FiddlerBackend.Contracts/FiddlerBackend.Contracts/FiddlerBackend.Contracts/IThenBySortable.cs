namespace FiddlerBackend.Contracts;

public interface IThenBySortable : ISortable
{
	string ThenBy { get; set; }
}
