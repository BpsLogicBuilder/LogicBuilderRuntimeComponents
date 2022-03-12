using System;

namespace LogicBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false)]
    public class SummaryAttribute : Attribute
    {
        public string Summary { get; private set; }
        /// <summary>
        /// Comments about the function.
        /// </summary>
        /// <param name="summary">Comments about the function.</param>
        public SummaryAttribute(string summary)
        {
            this.Summary = summary;
        }
    }
}
