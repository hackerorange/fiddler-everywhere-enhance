using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using FiddlerBackend.Contracts;
using FiddlerBackend.Contracts.DTO.Snapshots;
using FiddlerBackendSDK.AutoResponder.Notification;
using FiddlerBackendSDK.Comments;
using FiddlerBackendSDK.ComposerCollections.Client;
using FiddlerBackendSDK.ComposerCollections.Notification;
using FiddlerBackendSDK.Files.Client;
using FiddlerBackendSDK.License.Notification;
using FiddlerBackendSDK.Notifications;
using FiddlerBackendSDK.Snapshot.Client;
using FiddlerBackendSDK.Snapshot.Notification;

namespace FiddlerBackendSDK.Mapping;

public class AutoMapping : Profile
{
	public AutoMapping()
	{
		((IMappingExpressionBase<NotificationMessageDTO, NotificationMessage, IMappingExpression<NotificationMessageDTO, NotificationMessage>>)(object)((Profile)this).CreateMap<NotificationMessageDTO, NotificationMessage>()).IncludeAllDerived();
		((Profile)this).CreateMap<SnapshotCommentNotificationMessageDTO, SnapshotCommentNotificationMessage>();
		((Profile)this).CreateMap<NewSnapshotVersionMessageDTO, NewSnapshotVersionMessage>();
		((Profile)this).CreateMap<SnapshotDescriptionUpdatedMessageDTO, SnapshotDescriptionUpdatedMessage>();
		((Profile)this).CreateMap<SnapshotNameUpdatedMessageDTO, SnapshotNameUpdatedMessage>();
		((Profile)this).CreateMap<SnapshotNotificationMessageDTO, SnapshotNotificationMessage>();
		((Profile)this).CreateMap<SnapshotSharedMessageDTO, SnapshotSharedMessage>();
		((IMappingExpressionBase<RequestCommentDTO, RequestComment, IMappingExpression<RequestCommentDTO, RequestComment>>)(object)((Profile)this).CreateMap<RequestCommentDTO, RequestComment>()).IncludeAllDerived();
		((Profile)this).CreateMap<SnapshotRequestCommentDTO, SnapshotRequestComment>();
		((IMappingExpressionBase<RequestComment, RequestCommentDTO, IMappingExpression<RequestComment, RequestCommentDTO>>)(object)((Profile)this).CreateMap<RequestComment, RequestCommentDTO>()).IncludeAllDerived();
		((Profile)this).CreateMap<SnapshotRequestComment, SnapshotRequestCommentDTO>();
		((Profile)this).CreateMap<LocalSnapshotRequestComment, LocalSnapshotRequestCommentDTO>();
		((Profile)this).CreateMap<RuleSetNotificationMessageDTO, RuleSetNotificationMessage>();
		((Profile)this).CreateMap<RuleSetSharedMessageDTO, RuleSetSharedNotificationMessage>();
		((Profile)this).CreateMap<LicenseUpdatedNotificationMessageDTO, LicenseUpdatedNotificationMessage>();
		((Profile)this).CreateMap<ShareDTO, SnapshotShareReceiver>().ForMember<string>((Expression<Func<SnapshotShareReceiver, string>>)((SnapshotShareReceiver x) => x.Reason), (Action<IMemberConfigurationExpression<ShareDTO, SnapshotShareReceiver, string>>)delegate(IMemberConfigurationExpression<ShareDTO, SnapshotShareReceiver, string> opts)
		{
			((IProjectionMemberConfiguration<ShareDTO, SnapshotShareReceiver, string>)(object)opts).MapFrom<string>((Expression<Func<ShareDTO, string>>)((ShareDTO src) => src.Note));
		});
		((Profile)this).CreateMap<SnapshotVersionFileDTO, SnapshotVersionFileMetadata>();
		((Profile)this).CreateMap<FileDTO, RemoteFileMetadata>();
		((Profile)this).CreateMap<RemoteFileMetadata, FileDTO>();
		((Profile)this).CreateMap<BaseSnapshotDTO, BaseSnapshotMetadata>().ForMember<RemoteFileMetadata>((Expression<Func<BaseSnapshotMetadata, RemoteFileMetadata>>)((BaseSnapshotMetadata dest) => dest.SnapshotFile), (Action<IMemberConfigurationExpression<BaseSnapshotDTO, BaseSnapshotMetadata, RemoteFileMetadata>>)delegate(IMemberConfigurationExpression<BaseSnapshotDTO, BaseSnapshotMetadata, RemoteFileMetadata> opts)
		{
			((IProjectionMemberConfiguration<BaseSnapshotDTO, BaseSnapshotMetadata, RemoteFileMetadata>)(object)opts).MapFrom<FileDTO>((Expression<Func<BaseSnapshotDTO, FileDTO>>)((BaseSnapshotDTO src) => src.File));
		});
		((Profile)this).CreateMap<SnapshotDTO, SnapshotMetadata>().ForMember<RemoteFileMetadata>((Expression<Func<SnapshotMetadata, RemoteFileMetadata>>)((SnapshotMetadata dest) => dest.SnapshotFile), (Action<IMemberConfigurationExpression<SnapshotDTO, SnapshotMetadata, RemoteFileMetadata>>)delegate(IMemberConfigurationExpression<SnapshotDTO, SnapshotMetadata, RemoteFileMetadata> opts)
		{
			((IProjectionMemberConfiguration<SnapshotDTO, SnapshotMetadata, RemoteFileMetadata>)(object)opts).MapFrom<FileDTO>((Expression<Func<SnapshotDTO, FileDTO>>)((SnapshotDTO src) => src.File));
		}).ForMember<IEnumerable<SnapshotVersionFileMetadata>>((Expression<Func<SnapshotMetadata, IEnumerable<SnapshotVersionFileMetadata>>>)((SnapshotMetadata x) => x.SnapshotFileVersions), (Action<IMemberConfigurationExpression<SnapshotDTO, SnapshotMetadata, IEnumerable<SnapshotVersionFileMetadata>>>)delegate(IMemberConfigurationExpression<SnapshotDTO, SnapshotMetadata, IEnumerable<SnapshotVersionFileMetadata>> opts)
		{
			((IProjectionMemberConfiguration<SnapshotDTO, SnapshotMetadata, IEnumerable<SnapshotVersionFileMetadata>>)(object)opts).MapFrom<IList<SnapshotVersionFileDTO>>((Expression<Func<SnapshotDTO, IList<SnapshotVersionFileDTO>>>)((SnapshotDTO src) => src.Versions));
		});
		CreateComposerCollectionMappings();
	}

	private void CreateComposerCollectionMappings()
	{
		((Profile)this).CreateMap<ComposerCollectionRequestParentDTO, ComposerCollectionParent>();
		ComposerCollectionExtensions.CreateComposerCollectionMap<ComposerCollectionSharedMessageDTO, ComposerCollectionSharedNotificationMessage>((Profile)(object)this);
		ComposerCollectionExtensions.CreateComposerCollectionMap<ComposerCollectionResourceMovedMessageDTO, ComposerCollectionResourceMovedMessage>((Profile)(object)this);
		ComposerCollectionExtensions.CreateComposerCollectionMap<ComposerCollectionNameUpdatedMessageDTO, ComposerCollectionNameUpdatedMessage>((Profile)(object)this);
		ComposerCollectionExtensions.CreateComposerCollectionMap<ComposerCollectionNotificationMessageDTO, ComposerCollectionNotificationMessage>((Profile)(object)this);
		ComposerCollectionExtensions.CreateComposerCollectionMap<ComposerCollectionDeletedMessageDTO, ComposerCollectionDeletedMessage>((Profile)(object)this);
		ComposerCollectionExtensions.CreateComposerCollectionMap<ComposerCollectionFolderMessageDTO, ComposerCollectionFolderMessage>((Profile)(object)this);
		ComposerCollectionExtensions.CreateComposerCollectionMap<ComposerCollectionRequestMovedMessageDTO, ComposerCollectionRequestMovedMessage>((Profile)(object)this);
		ComposerCollectionExtensions.CreateComposerCollectionMap<ComposerCollectionRequestUpdatedMessageDTO, ComposerCollectionRequestUpdatedMessage>((Profile)(object)this);
		ComposerCollectionExtensions.CreateComposerCollectionMap<ComposerCollectionRequestMessageDTO, ComposerCollectionRequestMessage>((Profile)(object)this);
		ComposerCollectionExtensions.CreateComposerCollectionMap<ComposerCollectionFolderMovedMessageDTO, ComposerCollectionFolderMovedMessage>((Profile)(object)this);
		ComposerCollectionExtensions.CreateComposerCollectionMap<ComposerCollectionFolderNameUpdatedMessageDTO, ComposerCollectionFolderNameUpdatedMessage>((Profile)(object)this);
		((IMappingExpressionBase<FileDTO, IBlobResource<string>, IMappingExpression<FileDTO, IBlobResource<string>>>)(object)((Profile)this).CreateMap<FileDTO, IBlobResource<string>>()).ConvertUsing<FileDTOToBlobConverter>();
		((Profile)this).CreateMap<ComposerCollectionDTO, ComposerCollection>();
		((Profile)this).CreateMap<ComposerCollectionDTO, ComposerCollectionCacheItem>();
		((Profile)this).CreateMap<ComposerCollection, ComposerCollectionCacheItem>();
		((Profile)this).CreateMap<ComposerCollectionFolderDTO, ComposerCollectionFolder>();
		((Profile)this).CreateMap<ComposerCollectionRequestDTO, ComposerCollectionRequest>().ForMember<IBlobResource<string>>((Expression<Func<ComposerCollectionRequest, IBlobResource<string>>>)((ComposerCollectionRequest x) => x.RequestBodyFile), (Action<IMemberConfigurationExpression<ComposerCollectionRequestDTO, ComposerCollectionRequest, IBlobResource<string>>>)delegate(IMemberConfigurationExpression<ComposerCollectionRequestDTO, ComposerCollectionRequest, IBlobResource<string>> opts)
		{
			((IProjectionMemberConfiguration<ComposerCollectionRequestDTO, ComposerCollectionRequest, IBlobResource<string>>)(object)opts).MapFrom<FileDTO>((Expression<Func<ComposerCollectionRequestDTO, FileDTO>>)((ComposerCollectionRequestDTO src) => src.RequestBodyFile));
		}).ForMember<IBlobResource<string>>((Expression<Func<ComposerCollectionRequest, IBlobResource<string>>>)((ComposerCollectionRequest x) => x.RequestHeadersFile), (Action<IMemberConfigurationExpression<ComposerCollectionRequestDTO, ComposerCollectionRequest, IBlobResource<string>>>)delegate(IMemberConfigurationExpression<ComposerCollectionRequestDTO, ComposerCollectionRequest, IBlobResource<string>> opts)
		{
			((IProjectionMemberConfiguration<ComposerCollectionRequestDTO, ComposerCollectionRequest, IBlobResource<string>>)(object)opts).MapFrom<FileDTO>((Expression<Func<ComposerCollectionRequestDTO, FileDTO>>)((ComposerCollectionRequestDTO src) => src.RequestHeadersFile));
		});
		((Profile)this).CreateMap<ShareDTO, ComposerCollectionShareReceiver>().ForMember<string>((Expression<Func<ComposerCollectionShareReceiver, string>>)((ComposerCollectionShareReceiver x) => x.Reason), (Action<IMemberConfigurationExpression<ShareDTO, ComposerCollectionShareReceiver, string>>)delegate(IMemberConfigurationExpression<ShareDTO, ComposerCollectionShareReceiver, string> opts)
		{
			((IProjectionMemberConfiguration<ShareDTO, ComposerCollectionShareReceiver, string>)(object)opts).MapFrom<string>((Expression<Func<ShareDTO, string>>)((ShareDTO src) => src.Note));
		});
	}
}
