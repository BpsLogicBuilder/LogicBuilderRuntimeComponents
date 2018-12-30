using System;

namespace LogicBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]
    public class NameValueAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        /// <summary>
        /// Used to define additional name and valyue pairs useful to the field or property.
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        public NameValueAttribute(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
}
