using System;

namespace LogicBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ErrorMessageAttribute : Attribute
    {
        public string ErrorMessage { get; private set; }
        /// <summary>
        /// Use this attribute to configure a generic error message for the field or property.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        public ErrorMessageAttribute(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }
    }
}
