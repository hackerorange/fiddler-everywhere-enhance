using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.ComposerCollections.Client;

public interface IComposerCollectionClient
{
	Task<IList<ComposerCollection>> GetAvailableComposerCollectionsAsync();

	Task<ComposerCollection> GetComposerCollectionAsync(Guid composerCollectionId);

	Task<ComposerCollection> CreateComposerCollectionAsync(Guid accountId, ComposerCollection composerCollection);

	Task<ComposerCollection> UpdateComposerCollectionEmailsAsync(Guid composerCollectionId, IEnumerable<string> newEmails, string reason, string concurrencyToken);

	Task<ComposerCollection> UpdateComposerCollectionNameAsync(Guid composerCollectionId, string name, string concurrencyToken);

	Task DeleteComposerCollectionAsync(Guid composerCollectionId, string concurrencyToken);

	Task<ComposerCollection> CreateComposerCollectionFoldersAndRequests(Guid composerCollectionId, ComposerCollectionFolder folderToCreate, IEnumerable<ComposerCollectionFolder> childFolders, IEnumerable<ComposerCollectionRequest> childRequests, string concurrencyToken);

	Task<ComposerCollectionRequest> MoveComposerCollectionRequestAsync(Guid requestId, Guid sourceCollectionId, Guid targetCollectionId, Guid? targetFolderId, string concurrencyToken);

	Task<ComposerCollectionRequest> CreateComposerCollectionRequestAsync(Guid composerCollectionId, ComposerCollectionRequest requestToCreate, string concurrencyToken);

	Task DeleteComposerCollectionRequestAsync(Guid composerCollectionId, Guid requestId, string concurrencyToken);

	Task<ComposerCollectionRequest> UpdateComposerCollectionRequestAsync(Guid composerCollectionId, Guid requestId, string concurrencyToken, string name = null);

	Task<ComposerCollectionRequest> UpdateComposerCollectionRequestAsync(Guid composerCollectionId, ComposerCollectionRequest composerCollectionRequest, string concurrencyToken);

	Task<ComposerCollectionDiff> MoveComposerCollectionFolderAsync(Guid folderId, Guid sourceCollectionId, Guid targetCollectionId, Guid? targetFolderId, string concurrencyToken);

	Task DeleteComposerCollectionFolderAsync(Guid composerCollectionId, Guid folderId, string concurrencyToken);

	Task<ComposerCollectionFolder> UpdateComposerCollectionFolderAsync(Guid composerCollectionId, Guid folderId, string concurrencyToken, string name = null, string description = null);
}
