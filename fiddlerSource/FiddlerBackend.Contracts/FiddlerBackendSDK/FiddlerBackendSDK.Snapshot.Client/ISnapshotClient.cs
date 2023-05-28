using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.Snapshot.Client;

public interface ISnapshotClient
{
	Task<IList<SnapshotMetadata>> GetOwnSnapshotsAsync(bool includeDeleted = true);

	Task<IList<SnapshotMetadata>> GetSnapshotsSharedWithMeAsync(bool includeDeleted = true);

	Task<IList<SnapshotMetadata>> GetAvailableSnapshotsAsync(bool includeDeleted = true);

	Task<SnapshotMetadata> GetSnapshotAsync(Guid snapshotId, bool includeDeleted = true);

	Task<SnapshotMetadata> UploadSnapshotAsync(Guid accountId, Guid snapshotId, string filePath, string snapshotName, string snapshotDescription = null, IEnumerable<SnapshotRequestComment> comments = null, bool isPasswordProtected = false);

	Task<SnapshotMetadata> UpdateSnapshotEmailsAsync(Guid snapshotId, IEnumerable<string> newEmails, string reason, string concurrencyToken);

	Task<SnapshotMetadata> UpdateSnapshotNameAsync(Guid snapshotId, string name, string concurrencyToken);

	Task<SnapshotMetadata> UpdateSnapshotNameAndDescriptionAsync(Guid snapshotId, string name, string description, string concurrencyToken);

	Task DeleteSnapshotAsync(Guid snapshotId, string concurrencyToken);

	Task DownloadSnapshotAsync(Guid snapshotId, string filePath, string password = null);

	Task<SnapshotMetadata> UpdateSnapshotFileAsync(Guid snapshotId, string filePath, string concurrencyToken, string password = null);

	Task<IEnumerable<SnapshotRequestComment>> GetAllCommentsForSnapshotAsync(Guid snapshotId);

	Task<SnapshotRequestComment> CreateCommentOrReplyAsync(Guid snapshotId, Guid snapshotRequestId, string text, Guid? parentCommentId = null);

	Task<SnapshotRequestComment> UpdateCommentOrReplyAsync(Guid commentOrReplyId, string text);

	Task DeleteCommentOrReplyAsync(Guid commentOrReplyId);

	Task<SnapshotMetadata> UpdateSnapshotPasswordAsync(Guid snapshotId, string oldPassword, string newPassword, string targetPath = null);
}
