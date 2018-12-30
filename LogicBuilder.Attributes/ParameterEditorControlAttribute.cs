using System;

namespace LogicBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class ParameterEditorControlAttribute : Attribute
    {
        public ParameterControlType ControlType { get; private set; }
        /// <summary>
        /// Control to be used in BPS Logic Builder for the parameter.
        /// </summary>
        /// <param name="controlType"></param>
        public ParameterEditorControlAttribute(ParameterControlType controlType)
        {
            this.ControlType = controlType;
        }
    }
}
