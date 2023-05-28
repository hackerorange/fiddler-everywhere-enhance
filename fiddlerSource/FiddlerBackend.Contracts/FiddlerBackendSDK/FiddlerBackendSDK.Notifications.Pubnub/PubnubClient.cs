using System;
using System.Net;
using System.Threading.Tasks;
using PubnubApi;

namespace FiddlerBackendSDK.Notifications.Pubnub;

public class PubnubClient : IPubnubClient
{
	private readonly Pubnub pubnub;

	private readonly bool waitForSubscriptionConfirmation;

	public PubnubClient(string subscribeKey, string uniqueClientId, IWebProxy proxy, bool waitForSubscriptionConfirmation)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		UserId val = UserId.op_Implicit(uniqueClientId);
		PNConfiguration val2 = new PNConfiguration(val)
		{
			SubscribeKey = subscribeKey,
			Secure = true,
			UserId = val,
			ReconnectionPolicy = (PNReconnectionPolicy)1,
			Proxy = (Proxy)(object)new PubnubProxy(proxy),
			Origin = "fiddler.pubnubapi.com"
		};
		pubnub = new Pubnub(val2);
		this.waitForSubscriptionConfirmation = waitForSubscriptionConfirmation;
	}

	public void AddListener(SubscribeCallback subscribeCallback)
	{
		pubnub.AddListener(subscribeCallback);
	}

	public void RemoveListener(SubscribeCallback subscribeCallback)
	{
		pubnub.RemoveListener(subscribeCallback);
	}

	public void Subscribe(string channelName)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		if (waitForSubscriptionConfirmation)
		{
			bool flag = false;
			int num = 0;
			do
			{
				TaskCompletionSource<bool> subscribeConfirmedPromise = new TaskCompletionSource<bool>();
				SubscribeCallbackExt val = new SubscribeCallbackExt((Action<Pubnub, PNMessageResult<object>>)delegate
				{
				}, (Action<Pubnub, PNPresenceEventResult>)delegate
				{
				}, (Action<Pubnub, PNStatus>)delegate(Pubnub pubnubObj, PNStatus status)
				{
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					//IL_001f: Invalid comparison between Unknown and I4
					//IL_0028: Unknown result type (might be due to invalid IL or missing references)
					//IL_002e: Invalid comparison between Unknown and I4
					if (status.AffectedChannels.Contains(channelName) && (int)status.Operation == 1)
					{
						subscribeConfirmedPromise.SetResult((int)status.Category == 5);
					}
				});
				pubnub.AddListener((SubscribeCallback)(object)val);
				pubnub.Subscribe<object>().Channels(new string[1] { channelName }).Execute();
				Task<Task> task = Task.Run(async () => await Task.WhenAny(new Task[2]
				{
					subscribeConfirmedPromise.Task,
					Task.Delay(5000)
				}));
				task.Wait();
				flag = task.Result == subscribeConfirmedPromise.Task && subscribeConfirmedPromise.Task.Result;
				pubnub.RemoveListener((SubscribeCallback)(object)val);
				num++;
			}
			while (!flag && num <= 4);
			if (!flag)
			{
				throw new PubNubConnectionException($"Could not subscribe to PubNub in {num} tries");
			}
		}
		else
		{
			pubnub.Subscribe<object>().Channels(new string[1] { channelName }).Execute();
		}
	}

	public void Unsubscribe(string channelName)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		if (waitForSubscriptionConfirmation)
		{
			bool flag = false;
			int num = 0;
			do
			{
				TaskCompletionSource<bool> unsubscribeConfirmedPromise = new TaskCompletionSource<bool>();
				SubscribeCallbackExt val = new SubscribeCallbackExt((Action<Pubnub, PNMessageResult<object>>)delegate
				{
				}, (Action<Pubnub, PNPresenceEventResult>)delegate
				{
				}, (Action<Pubnub, PNStatus>)delegate(Pubnub pubnubObj, PNStatus status)
				{
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					//IL_001f: Invalid comparison between Unknown and I4
					//IL_0028: Unknown result type (might be due to invalid IL or missing references)
					//IL_002e: Invalid comparison between Unknown and I4
					if (status.AffectedChannels.Contains(channelName) && (int)status.Operation == 2)
					{
						unsubscribeConfirmedPromise.SetResult((int)status.Category == 7);
					}
				});
				pubnub.AddListener((SubscribeCallback)(object)val);
				pubnub.Unsubscribe<object>().Channels(new string[1] { channelName }).Execute();
				Task<Task> task = Task.Run(async () => await Task.WhenAny(new Task[2]
				{
					unsubscribeConfirmedPromise.Task,
					Task.Delay(5000)
				}));
				task.Wait();
				flag = task.Result == unsubscribeConfirmedPromise.Task;
				pubnub.RemoveListener((SubscribeCallback)(object)val);
				num++;
			}
			while (!flag && num <= 3);
			if (!flag)
			{
				throw new PubNubConnectionException($"Could not unsubscribe from PubNub in {num} tries");
			}
		}
		else
		{
			pubnub.Unsubscribe<object>().Channels(new string[1] { channelName }).Execute();
		}
	}
}
