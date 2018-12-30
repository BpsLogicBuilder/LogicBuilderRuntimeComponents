using System.Globalization;

namespace LogicBuilder.RulesDirector
{
    public static class ResourcesHelper<T>
    {
        public static T GetResource(string shortValue, DirectorBase director)
        {
            object result;
            if (director.RulesCache.ResourceStrings.TryGetValue(shortValue, out string longValue))
            {
                if (!longValue.TryParse(typeof(T), out result))
                    throw new DirectorException(string.Format(CultureInfo.InvariantCulture, Strings.getResourceFailedInvalidFormat, typeof(T).ToString(), shortValue, longValue));
            }
            else
            {
                if (!shortValue.TryParse(typeof(T), out result))
                    throw new DirectorException(string.Format(CultureInfo.InvariantCulture, Strings.getResourceFailedNotFoundFormat, shortValue));
            }

            return (T)result;
        }
    }
}
