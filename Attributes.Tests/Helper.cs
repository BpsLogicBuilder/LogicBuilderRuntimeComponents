using System;
using System.Linq;
using System.Reflection;

namespace Attributes.Tests
{
    internal static class Helper
    {
        internal static Attribute GetAttribute(MemberInfo memberInfo, string attributeName)
        {
            return (Attribute)memberInfo
                .GetCustomAttributes(true)
                .Single(attribute => attribute.GetType().FullName == attributeName);
        }
    }
}
