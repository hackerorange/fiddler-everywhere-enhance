using System;

namespace FiddlerBackendSDK.Notifications.Pubnub;

public class PubNubConnectionException : Exception
{
	public PubNubConnectionException(string message)
		: base(message)
	{
	}
}
