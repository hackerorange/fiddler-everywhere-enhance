using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class CreateJAMSessionVideoDTO
{
	public long? TabId { get; set; }

	public byte[] EncryptionKeyId { get; set; }

	public string MimeType { get; set; }

	public decimal Duration { get; set; }

	public bool IsDomRecording { get; set; }

	public Guid FileId { get; set; }

	public CreateJAMSessionVideoInitClusterDTO Init { get; set; }

	public ICollection<CreateJAMSessionVideoMediaClusterDTO> Media { get; set; } = new List<CreateJAMSessionVideoMediaClusterDTO>();

}
