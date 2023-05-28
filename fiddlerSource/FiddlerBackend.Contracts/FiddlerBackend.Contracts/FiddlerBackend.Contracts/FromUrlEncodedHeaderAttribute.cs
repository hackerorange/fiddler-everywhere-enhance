using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FiddlerBackend.Contracts;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
public class FromUrlEncodedHeaderAttribute : Attribute, IBindingSourceMetadata, IModelNameProvider
{
	public class UrlEncodedHeaderBindingSource : BindingSource
	{
		public static readonly BindingSource UrlEncodedHeader = (BindingSource)(object)new UrlEncodedHeaderBindingSource("UrlEncodedHeader", "UrlEncodedHeader", isGreedy: true, isFromRequest: true);

		public UrlEncodedHeaderBindingSource(string id, string displayName, bool isGreedy, bool isFromRequest)
			: base(id, displayName, isGreedy, isFromRequest)
		{
		}

		public override bool CanAcceptDataFrom(BindingSource bindingSource)
		{
			if (!(bindingSource == BindingSource.Header))
			{
				return bindingSource == (BindingSource)(object)this;
			}
			return true;
		}
	}

	public class UrlEncodedHeaderModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			string text = ((IEnumerable<string>)(object)bindingContext.HttpContext.Request.Headers[bindingContext.ModelName]).FirstOrDefault();
			if (!string.IsNullOrEmpty(text))
			{
				text = WebUtility.UrlDecode(text);
			}
			bindingContext.Result = ModelBindingResult.Success((object)text);
			return Task.CompletedTask;
		}
	}

	public class UrlEncodedHeaderModelBinderProvider : IModelBinderProvider
	{
		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			if (context.BindingInfo.BindingSource != (BindingSource)null && context.BindingInfo.BindingSource.CanAcceptDataFrom(UrlEncodedHeaderBindingSource.UrlEncodedHeader))
			{
				return (IModelBinder)(object)new UrlEncodedHeaderModelBinder();
			}
			return null;
		}
	}

	public BindingSource BindingSource => UrlEncodedHeaderBindingSource.UrlEncodedHeader;

	public string Name { get; set; }
}
