using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class JAMSessionRequestDTO : JAMSessionArtifactDTO
{
	public string Url { get; set; }

	public string HttpVerb { get; set; }

	public int StatusCode { get; set; }

	public bool? IsFileDownload { get; set; }

	public bool? ResponseNotReceived { get; set; }

	public DateTime? DownloadStartTime { get; set; }

	public DateTime? DownloadEndTime { get; set; }

	public long? DownloadTotalBytes { get; set; }

	public string DownloadState { get; set; }

	public string DownloadInterruptReason { get; set; }

	public IList<RequestCommentDTO> Comments { get; set; } = new List<RequestCommentDTO>();

}
