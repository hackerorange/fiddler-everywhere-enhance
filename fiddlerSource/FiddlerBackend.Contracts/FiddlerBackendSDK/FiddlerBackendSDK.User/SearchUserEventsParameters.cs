using System.Collections.Specialized;
using System.Web;
using FiddlerBackend.Contracts;

namespace FiddlerBackendSDK.User;

public class SearchUserEventsParameters
{
	public EventType? Type { get; set; } = EventType.Notification;


	public FiddlerProduct? Product { get; set; }

	public UserSearchTypes UserIs { get; set; } = UserSearchTypes.SenderOrReceiver;


	public bool IncludeRead { get; set; }

	public string OrderBy { get; set; } = "CreatedAt";


	public bool OrderDesc { get; set; } = true;


	public int Skip { get; set; }

	public int Take { get; set; } = 10;


	internal string GetQueryString()
	{
		NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(string.Empty);
		if (Type.HasValue)
		{
			nameValueCollection["type"] = Type.ToString();
		}
		if (Product.HasValue)
		{
			nameValueCollection["product"] = Product.ToString();
		}
		nameValueCollection["userIs"] = UserIs.ToString();
		if (IncludeRead)
		{
			nameValueCollection["includeRead"] = "true";
		}
		if (!string.IsNullOrWhiteSpace(OrderBy))
		{
			nameValueCollection["orderBy"] = OrderBy;
		}
		nameValueCollection["orderDesc"] = OrderDesc.ToString().ToLower();
		nameValueCollection["skip"] = Skip.ToString();
		nameValueCollection["take"] = Take.ToString();
		return nameValueCollection.ToString();
	}
}
