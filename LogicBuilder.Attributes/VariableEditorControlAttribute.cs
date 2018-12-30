using System;

namespace LogicBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class VariableEditorControlAttribute : Attribute
    {
        public VariableControlType ControlType { get; private set; }
        /// <summary>
        /// Control to be used in BPS Logic Builder for the field or property.
        /// </summary>
        /// <param name="controlType"></param>
        public VariableEditorControlAttribute(VariableControlType controlType)
        {
            this.ControlType = controlType;
        }
    }
}
