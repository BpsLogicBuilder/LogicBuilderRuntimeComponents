using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LogicBuilder.RulesDirector
{
    public static class VariablesHelper<T>
    {
        public static T GetVariable(string key, DirectorBase director)
        {
            if (!director.Variables.ValuesDictionary.TryGetValue(key, out VariableInfo vInfo))
            {
                director.Variables.ValuesDictionary.Add(key, new VariableInfo(key, typeof(T).ToString(), default(T)));
                return default(T);
            }

            if (vInfo.VariableType != typeof(T).ToString())
                throw new DirectorException(string.Format(CultureInfo.InvariantCulture, Strings.getFailedInvalidTypeFormat, typeof(T).ToString(), key, vInfo.VariableType));

            if (vInfo.VariableValue == null)
            {
                if (typeof(T).CanBeAssignedNull())
                    return default(T);
                else
                    throw new DirectorException(string.Format(CultureInfo.InvariantCulture, Strings.getFailedNullLiteralFormat, typeof(T).ToString(), key));
            }

            if (!typeof(T).AssignableFrom(vInfo.VariableValue.GetType()))
                throw new DirectorException(string.Format(CultureInfo.InvariantCulture, Strings.getFailedInvalidDataFormat, typeof(T).ToString(), key, vInfo.VariableValue.ToString()));

            return (T)vInfo.VariableValue;
        }

        public static void SetVariable(string key, T newValue, DirectorBase director)
        {
            if (director.Variables.ValuesDictionary.TryGetValue(key, out VariableInfo vInfo))
            {
                if (vInfo.VariableType != typeof(T).ToString())
                    throw new DirectorException(string.Format(CultureInfo.InvariantCulture, Strings.setFailedInvalidTypeFormat, typeof(T).ToString(), key, vInfo.VariableType));

                vInfo.VariableValue = newValue;
            }
            else
                director.Variables.ValuesDictionary.Add(key, new VariableInfo(key, typeof(T).ToString(), newValue));

            director.Variables.RaiseValuesChanged(new List<string>(new string[] { key }));
        }
    }
}
