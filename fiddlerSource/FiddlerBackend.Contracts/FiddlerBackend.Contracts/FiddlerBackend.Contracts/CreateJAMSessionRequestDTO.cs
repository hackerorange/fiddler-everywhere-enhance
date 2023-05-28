using System;

namespace FiddlerBackend.Contracts;

[ArtifactType("request")]
public class CreateJAMSessionRequestDTO : CreateJAMSessionArtifactDTO
{
	public override string Type { get; set; } = "request";


	public string Url { get; set; }

	public string HttpVerb { get; set; }

	public int? StatusCode { get; set; }

	public bool? IsFileDownload { get; set; }

	public bool? ResponseNotReceived { get; set; }

	public DateTime? DownloadStartTime { get; set; }

	public DateTime? DownloadEndTime { get; set; }

	public long? DownloadTotalBytes { get; set; }

	public string DownloadState { get; set; }

	public string DownloadInterruptReason { get; set; }

	public override void Validate()
	{
		base.Validate();
		if (!base.StartOffset.HasValue)
		{
			throw new ValidationException("StartOffset field is required", "The StartOffset field is required for creating a new JAM session request");
		}
		if (!base.EndOffset.HasValue)
		{
			throw new ValidationException("EndOffset field is required", "The EndOffset field is required for creating a new JAM session request");
		}
		if (string.IsNullOrEmpty(Url))
		{
			throw new ValidationException("Url field is required", "The Url field is required for creating a new JAM session request");
		}
		if (string.IsNullOrEmpty(HttpVerb))
		{
			throw new ValidationException("HttpVerb field is required", "The HttpVerb field is required for creating a new JAM session request");
		}
		if (!StatusCode.HasValue)
		{
			throw new ValidationException("StatusCode field is required", "The StatusCode field is required for creating a new JAM session request");
		}
	}
}
