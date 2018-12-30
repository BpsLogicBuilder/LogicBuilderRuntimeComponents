using System;
using System.Collections.Generic;
using System.Globalization;

namespace LogicBuilder.RulesDirector
{
    [Serializable()]
    public class Variables
    {
        public Variables()
        {
        }

        private Variables(Dictionary<string, VariableInfo> valuesDictionary)
        {
            this.valuesDictionary = valuesDictionary;
        }

        #region Constants
        #endregion Constants

        #region Variables
        private Dictionary<string, VariableInfo> valuesDictionary = new Dictionary<string, VariableInfo>();
        public event EventHandler ValuesChanged;
        #endregion Variables

        #region Properties
        public Dictionary<string, VariableInfo> ValuesDictionary
        {
            get { return valuesDictionary; }
        }
        #endregion Properties

        #region Methods
        public Variables Clone()
        {
            Variables clone = new Variables(new Dictionary<string, VariableInfo>());
            foreach (VariableInfo vInfo in valuesDictionary.Values)
                clone.ValuesDictionary.Add(vInfo.VariableName, vInfo.Clone());
            return clone;
        }

        private void SetVariable(string key, VariableInfo variableInfo)
        {
            if (valuesDictionary.TryGetValue(key, out VariableInfo vInfo))
            {
                if (vInfo.VariableType != variableInfo.VariableType)
                    throw new DirectorException(string.Format(CultureInfo.InvariantCulture, Strings.setFailedInvalidTypeFormat, variableInfo.VariableType, key, vInfo.VariableType));
                vInfo.VariableValue = variableInfo.VariableValue;
                vInfo.Tag = variableInfo.Tag;
                vInfo.Data1 = variableInfo.Data1;
                vInfo.Data2 = variableInfo.Data2;
                vInfo.Data3 = variableInfo.Data3;
            }
            else
                valuesDictionary.Add(key, variableInfo);
        }

        public void SetValues(Dictionary<string, VariableInfo> values)
        {
            if (values == null)
                return;

            List<string> keys = new List<string>();
            foreach (string key in values.Keys)
            {
                SetVariable(key, values[key]);
                keys.Add(key);
            }

            RaiseValuesChanged(keys);
        }

        internal void RaiseValuesChanged(List<string> keys)
        {
            ValuesChanged?.Invoke(keys, EventArgs.Empty);
        }
        #endregion Methods
    }
}
