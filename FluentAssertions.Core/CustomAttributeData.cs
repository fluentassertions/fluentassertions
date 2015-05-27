using System;
using System.Linq;

namespace FluentAssertions
{
#if CUSTOM_ATTRIBUTEDATA
	internal sealed class CustomAttributeData
	{
		internal Type AttributeType { get; private set; }

		private CustomAttributeData(Type attributeType)
		{
			AttributeType = attributeType;
		}

		internal static CustomAttributeData[] From(object[] customAttributes)
		{
			return customAttributes.Select(o => new CustomAttributeData(o.GetType())).ToArray();
		}
	}
#endif
}
