using System;
using AutoMapper;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.ComposerCollections.Client;
using FiddlerBackendSDK.ComposerCollections.Notification;

namespace FiddlerBackendSDK.Mapping;

public static class ComposerCollectionExtensions
{
	public static IMappingExpression<TSource, TDestination> CreateComposerCollectionMap<TSource, TDestination>(this Profile profile) where TSource : ComposerCollectionNotificationMessageDTO where TDestination : ComposerCollectionNotificationMessage
	{
		return ((IMappingExpressionBase<TSource, TDestination, IMappingExpression<TSource, TDestination>>)(object)profile.CreateMap<TSource, TDestination>()).AfterMap((Action<TSource, TDestination>)delegate(TSource source, TDestination dest)
		{
			if (dest != null)
			{
				if (dest.ComposerCollection.Folders != null)
				{
					foreach (ComposerCollectionFolder folder in dest.ComposerCollection.Folders)
					{
						folder.AccountId = source.ComposerCollection.AccountId;
						folder.Owner = source.ComposerCollection.Owner;
					}
				}
				if (dest.ComposerCollection.Requests != null)
				{
					foreach (ComposerCollectionRequest request in dest.ComposerCollection.Requests)
					{
						request.AccountId = source.ComposerCollection.AccountId;
						request.Owner = source.ComposerCollection.Owner;
					}
				}
			}
		});
	}
}
