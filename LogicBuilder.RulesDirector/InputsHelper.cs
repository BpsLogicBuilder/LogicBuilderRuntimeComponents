using System.Collections.Generic;
using System.Globalization;

namespace LogicBuilder.RulesDirector
{
    public static class InputsHelper<T>
    {
        public static T GetInput(DirectorBase director, int index)
        {
            if (director.InputQuestionsAnswers.TryGetValue(index, out object storedValue))
            {
                if (storedValue == null)
                    return default(T);

                if (!typeof(T).AssignableFrom(storedValue.GetType()))
                    throw new DirectorException(string.Format(CultureInfo.InvariantCulture, Strings.getInputFailedInvalidFormat, typeof(T).ToString(), index, storedValue));

                return (T)storedValue;
            }
            else
                throw new DirectorException(string.Format(CultureInfo.InvariantCulture, Strings.getInputFailedNotFoundFormat, index));
        }
    }
}
