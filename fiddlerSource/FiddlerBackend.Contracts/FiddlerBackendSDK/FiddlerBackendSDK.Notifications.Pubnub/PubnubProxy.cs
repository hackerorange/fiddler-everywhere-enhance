using System;
using System.Net;
using PubnubApi;

namespace FiddlerBackendSDK.Notifications.Pubnub;

internal class PubnubProxy : Proxy, IWebProxy
{
	private readonly IWebProxy proxy;

	ICredentials IWebProxy.Credentials
	{
		get
		{
			return proxy.Credentials;
		}
		set
		{
			proxy.Credentials = value;
		}
	}

	public PubnubProxy(IWebProxy proxy)
		: base((Uri)null)
	{
		this.proxy = proxy;
	}

	Uri IWebProxy.GetProxy(Uri destination)
	{
		return proxy.GetProxy(destination);
	}

	bool IWebProxy.IsBypassed(Uri host)
	{
		return proxy.IsBypassed(host);
	}
}
