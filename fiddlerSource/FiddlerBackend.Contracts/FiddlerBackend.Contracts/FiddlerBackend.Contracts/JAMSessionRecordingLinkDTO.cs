using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class JAMSessionRecordingLinkDTO
{
	public Guid Id { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public Uri PortalUrl { get; set; }

	public int CaptureSettings { get; set; }

	public string Title { get; set; }

	public string StartingUrl { get; set; }

	public string Message { get; set; }

	public int SessionsCount { get; set; }

	public int MaxAllowedSessions { get; set; }

	public virtual ICollection<string> SharedWith { get; set; } = new List<string>();

}
