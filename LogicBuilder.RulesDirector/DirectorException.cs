using System;
using System.Runtime.Serialization;

namespace LogicBuilder.RulesDirector
{
    /// <summary>
    /// Exceptions thrown from this application
    /// </summary>
    [Serializable()]
    public class DirectorException : System.Exception
    {
        public DirectorException()
            : base(Strings.defaultErrorMessage)
        {
            // Add any type-specific logic, and supply the default message.
        }


        public DirectorException(string message)
            : base(message)
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DirectorException(string message, Exception ex)
            : base(message, ex)
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected DirectorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Implement type-specific serialization constructor logic.
        }
    }
}
