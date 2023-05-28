using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class JAMSessionVideoDTO
{
	public long? TabId { get; set; }

	public byte[] EncryptionKeyId { get; set; }

	public string MimeType { get; set; }

	public decimal Duration { get; set; }

	public bool IsDomRecording { get; set; }

	public FileDTO File { get; set; }

	public JAMSessionVideoInitClusterDTO Init { get; set; }

	public IList<JAMSessionVideoMediaClusterDTO> Media { get; set; } = new List<JAMSessionVideoMediaClusterDTO>();

}
