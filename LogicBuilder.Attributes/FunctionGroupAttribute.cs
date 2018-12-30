using System;

namespace LogicBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class FunctionGroupAttribute : Attribute
    {
        public FunctionGroup FunctionGroup { get; private set; }
        /// <summary>
        /// BPS Logic Builder function category.
        /// </summary>
        /// <param name="functionGroup"></param>
        public FunctionGroupAttribute(FunctionGroup functionGroup)
        {
            this.FunctionGroup = functionGroup;
        }
    }
}
