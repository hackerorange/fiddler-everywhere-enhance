using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FiddlerBackend.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FiddlerBackendSDK.Notifications.Serialization;

public class NotificationMessageDeserializer : INotificationMessageDeserializer
{
	private static readonly Regex FindOperationRegex = new Regex("\\\"Operation\\\"\\s*\\:\\s*\\\"(?<Operation>.*?)\\\"", RegexOptions.Compiled);

	public NotificationMessageDTO Deserialize(string payload)
	{
		Match match = FindOperationRegex.Match(payload);
		if (!match.Success || string.IsNullOrWhiteSpace(match.Groups["Operation"].Value))
		{
			return null;
		}
		return Deserialize(match.Groups["Operation"].Value, (Type type) => JsonConvert.DeserializeObject(payload, type));
	}

	public NotificationMessageDTO Deserialize(JObject payload)
	{
		string text = (payload.ContainsKey("Operation") ? Extensions.Value<string>((IEnumerable<JToken>)payload["Operation"]) : null);
		if (!string.IsNullOrWhiteSpace(text))
		{
			return Deserialize(text, (Func<Type, object>)((JToken)payload).ToObject);
		}
		return null;
	}

	private static NotificationMessageDTO Deserialize(string operation, Func<Type, object> deserializeOp)
	{
		switch (operation)
		{
		case "AccountActivated":
		case "AccountDeactivated":
		case "AccountCancelled":
		case "AccountResumed":
		case "AccountUpgraded":
		case "AccountMigrated":
			return (NotificationMessageDTO)deserializeOp(typeof(LicenseUpdatedNotificationMessageDTO));
		case "SnapshotUpdated":
			return (NotificationMessageDTO)deserializeOp(typeof(NewSnapshotVersionMessageDTO));
		case "SnapshotNameUpdated":
			return (NotificationMessageDTO)deserializeOp(typeof(SnapshotNameUpdatedMessageDTO));
		case "SnapshotDescriptionUpdated":
			return (NotificationMessageDTO)deserializeOp(typeof(SnapshotDescriptionUpdatedMessageDTO));
		case "SnapshotPasswordUpdated":
			return (NotificationMessageDTO)deserializeOp(typeof(SnapshotNotificationMessageDTO));
		case "SnapshotShared":
			return (NotificationMessageDTO)deserializeOp(typeof(SnapshotSharedMessageDTO));
		case "RuleSetShared":
			return (NotificationMessageDTO)deserializeOp(typeof(RuleSetSharedMessageDTO));
		case "ResourceMoved":
			return (NotificationMessageDTO)deserializeOp(typeof(ResourceMovedNotificationMessageDTO));
		default:
			if (operation.StartsWith("CommentInSnapshot"))
			{
				return (NotificationMessageDTO)deserializeOp(typeof(SnapshotCommentNotificationMessageDTO));
			}
			if (operation.StartsWith("Snapshot"))
			{
				return (NotificationMessageDTO)deserializeOp(typeof(SnapshotNotificationMessageDTO));
			}
			if (operation.StartsWith("RuleSet"))
			{
				return (NotificationMessageDTO)deserializeOp(typeof(RuleSetNotificationMessageDTO));
			}
			if (operation.StartsWith("ComposerCollection"))
			{
				return HandleComposerCollectionMessage(operation, deserializeOp);
			}
			return (NotificationMessageDTO)deserializeOp(typeof(NotificationMessageDTO));
		}
	}

	private static NotificationMessageDTO HandleComposerCollectionMessage(string operation, Func<Type, object> deserializeOp)
	{
		return operation switch
		{
			"ComposerCollectionShared" => (NotificationMessageDTO)deserializeOp(typeof(ComposerCollectionSharedMessageDTO)), 
			"ComposerCollectionUnshared" => (NotificationMessageDTO)deserializeOp(typeof(ComposerCollectionSharedMessageDTO)), 
			"ComposerCollectionNameUpdated" => (NotificationMessageDTO)deserializeOp(typeof(ComposerCollectionNameUpdatedMessageDTO)), 
			"ComposerCollectionDeleted" => (NotificationMessageDTO)deserializeOp(typeof(ComposerCollectionDeletedMessageDTO)), 
			"ComposerCollectionRequestDeleted" => (NotificationMessageDTO)deserializeOp(typeof(ComposerCollectionRequestMessageDTO)), 
			"ComposerCollectionRequestMoved" => (NotificationMessageDTO)deserializeOp(typeof(ComposerCollectionRequestMovedMessageDTO)), 
			"ComposerCollectionRequestUpdated" => (NotificationMessageDTO)deserializeOp(typeof(ComposerCollectionRequestUpdatedMessageDTO)), 
			"ComposerCollectionRequestCreated" => (NotificationMessageDTO)deserializeOp(typeof(ComposerCollectionRequestMessageDTO)), 
			"ComposerCollectionFolderDeleted" => (NotificationMessageDTO)deserializeOp(typeof(ComposerCollectionFolderMessageDTO)), 
			"ComposerCollectionFolderMoved" => (NotificationMessageDTO)deserializeOp(typeof(ComposerCollectionFolderMovedMessageDTO)), 
			"ComposerCollectionFolderNameUpdated" => (NotificationMessageDTO)deserializeOp(typeof(ComposerCollectionFolderNameUpdatedMessageDTO)), 
			"ComposerCollectionFolderCreated" => (NotificationMessageDTO)deserializeOp(typeof(ComposerCollectionFolderMessageDTO)), 
			_ => (NotificationMessageDTO)deserializeOp(typeof(ComposerCollectionNotificationMessageDTO)), 
		};
	}
}
