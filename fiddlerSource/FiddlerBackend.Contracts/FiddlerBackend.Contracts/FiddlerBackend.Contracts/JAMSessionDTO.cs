using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class JAMSessionDTO
{
	public Guid Id { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public DateTime? ScheduledForRemovalAt { get; set; }

	public string Owner { get; set; }

	public string Domain { get; set; }

	public string StartingUrl { get; set; }

	public DateTime? StartedAt { get; set; }

	public DateTime? StoppedAt { get; set; }

	public string Description { get; set; }

	public bool HasPassword { get; set; }

	public byte[] FilesEncryptionSalt { get; set; }

	public uint? FilesEncryptionIterations { get; set; }

	public string ExtensionVersion { get; set; }

	public string ChromeVersion { get; set; }

	public string AppVersion { get; set; }

	public string UserAgent { get; set; }

	public int? ScreenWidth { get; set; }

	public int? ScreenHeight { get; set; }

	public int? AvailableWidth { get; set; }

	public int? AvailableHeight { get; set; }

	public int? ScreenDPI { get; set; }

	public double? DevicePixelRatio { get; set; }

	public bool? IsRetinaDisplay { get; set; }

	public string Timezone { get; set; }

	public string Locale { get; set; }

	public JAMSessionCaptureSettings? CaptureSettings { get; set; }

	public bool? IsDeletable { get; set; }

	public bool IsSample { get; set; }

	public string Title { get; set; }

	public FileDTO File { get; set; }

	public FileDTO ScreenshotsFile { get; set; }

	public FileDTO StorageInfoFile { get; set; }

	public JAMSessionExternalIssueDTO ExternalIssue { get; set; }

	public IList<JAMSessionVideoDTO> Videos { get; set; } = new List<JAMSessionVideoDTO>();


	public IList<Guid> WorkspaceIds { get; set; } = new List<Guid>();

}
