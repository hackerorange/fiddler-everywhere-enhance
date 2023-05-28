using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class CreateJAMSessionDTO
{
	public string UploadToken { get; set; }

	public string SessionRecordingLinkId { get; set; }

	public Guid? WorkspaceId { get; set; }

	public string Domain { get; set; }

	public string Password { get; set; }

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

	public Guid FileId { get; set; }

	public Guid? ScreenshotsFileId { get; set; }

	public Guid? StorageInfoFileId { get; set; }

	public Guid? FontsInfoFileId { get; set; }

	public string Description { get; set; }

	public string StartingUrl { get; set; }

	public DateTime? StartedAt { get; set; }

	public DateTime? StoppedAt { get; set; }

	public string SubmittedBy { get; set; }

	public IList<string> SharedWithEmails { get; set; } = new List<string>();


	public IList<CreateJAMSessionVideoDTO> Videos { get; set; } = new List<CreateJAMSessionVideoDTO>();

}
