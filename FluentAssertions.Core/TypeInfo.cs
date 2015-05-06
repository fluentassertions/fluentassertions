using System;
using System.Reflection;

namespace FluentAssertions
{
#if NO_TYPEINFO

	internal sealed class TypeInfo
	{
		private readonly Type type;
		internal TypeInfo(Type type)
		{
			this.type = type;
		}

		public Assembly Assembly { get { return type.Assembly; } }

		public CustomAttributeData[] CustomAttributes
		{
			get
			{
#if CUSTOM_ATTRIBUTEDATA
				return CustomAttributeData.From(type.GetCustomAttributes(false));
#endif
			}
		}

		public bool IsEnum { get { return type.IsEnum; } }

		public bool IsGenericType { get { return type.IsGenericType; } }

		public bool IsInterface {  get { return type.IsInterface; } }

		public Type[] GenericTypeArguments {  get { return type.GetGenericArguments(); } }

		public bool IsAssignableFrom(TypeInfo c)
		{
			return type.IsAssignableFrom(c.type);
		}

		public object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return type.GetCustomAttributes(attributeType, inherit);
		}
    }
#endif
}
