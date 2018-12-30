using System;

namespace LogicBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ControlAttribute : Attribute
    {
        public string ControlName { get; private set; }
        /// <summary>
        /// Control Attribute defines the control to be used in the business application
        /// </summary>
        /// <param name="controlName">Name of the control</param>
        public ControlAttribute(string controlName)
        {
            this.ControlName = controlName;
        }
    }
}
