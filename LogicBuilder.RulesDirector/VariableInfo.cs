using System;
using System.Globalization;

namespace LogicBuilder.RulesDirector
{
    [Serializable]
    [Obsolete]
    public class VariableInfo : IEquatable<VariableInfo>, IComparable<VariableInfo>
    {
        public VariableInfo(string variableName, string variableType, object variableValue)
        {
            this.variableName = variableName;
            this.variableType = variableType;
            this.variableValue = variableValue;
        }

        public VariableInfo(string variableName, string variableType, object variableValue, object tag)
        {
            this.variableName = variableName;
            this.variableType = variableType;
            this.variableValue = variableValue;
            this.tag = tag;
        }

        #region Variables
        private string variableName;
        private string variableType;
        private object variableValue;
        private object tag;
        private object data1;
        private object data2;
        private object data3;
        #endregion Variables

        #region Properties
        public string VariableName
        {
            get { return variableName; }
        }

        public string VariableType
        {
            get { return variableType; }
            set { variableType = value; }
        }

        public object VariableValue
        {
            get { return variableValue; }
            set { variableValue = value; }
        }

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public object Data1
        {
            get { return data1; }
            set { data1 = value; }
        }

        public object Data2
        {
            get { return data2; }
            set { data2 = value; }
        }

        public object Data3
        {
            get { return data3; }
            set { data3 = value; }
        }
        #endregion Properties

        #region Methods
        public VariableInfo Clone()
        {
            VariableInfo clone = new VariableInfo(this.variableName, this.variableType, this.variableValue)
            {
                tag = this.tag,
                data1 = this.data1,
                data2 = this.data2,
                data3 = this.data3
            };
            return clone;
        }

        public override int GetHashCode()
        {
            return this.variableName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType())
                return false;

            VariableInfo other = (VariableInfo)obj;
            return this.variableName == other.variableName
                && this.variableType == other.variableType
                && this.variableValue == other.variableValue;
        }

        #region IEquatable<VariableInfo> Members
        public bool Equals(VariableInfo other)
        {
            if (other == null)
                return false;

            return this.variableName == other.variableName
                && this.variableType == other.variableType
                && this.variableValue == other.variableValue;
        }
        #endregion

        #region IComparable<VariableInfo> Members
        public int CompareTo(VariableInfo other)
        {
            if (other == null)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Strings.invalidArgumentFormat, "{5C9004E5-C26F-4842-B7A4-BD5DED6A0F63}"));

            return this.variableName.CompareTo(other.variableName);
        }
        #endregion

        public override string ToString()
        {
            Type type = Type.GetType(this.variableType);
            string vName = type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))
                        ? Strings.ResourceManager.GetString(Constants.DISPLAYNULLABLEPREFIX + Nullable.GetUnderlyingType(type).Name)
                        : Strings.ResourceManager.GetString(Constants.DISPLAYPREFIX + type.Name);

            return string.Format(CultureInfo.CurrentCulture, Strings.variableTypeToStringFormat, this.variableName, vName);
        }
        #endregion Methods
    }
}
