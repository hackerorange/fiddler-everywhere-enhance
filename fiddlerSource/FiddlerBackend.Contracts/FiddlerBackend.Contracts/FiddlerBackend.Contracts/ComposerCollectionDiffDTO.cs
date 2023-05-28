namespace FiddlerBackend.Contracts;

public class ComposerCollectionDiffDTO
{
	public ComposerCollectionDTO OldCollection { get; set; }

	public ComposerCollectionDTO NewCollection { get; set; }
}
