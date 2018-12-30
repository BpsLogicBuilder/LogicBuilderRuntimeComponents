using System;

namespace LogicBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ListEditorControlAttribute : Attribute
    {
        public ListControlType ControlType { get; private set; }
        /// <summary>
        /// Control to be used in BPS Logic Builder for the list parameter.
        /// </summary>
        /// <param name="controlType"></param>
        public ListEditorControlAttribute(ListControlType controlType)
        {
            this.ControlType = controlType;
        }
    }
}
