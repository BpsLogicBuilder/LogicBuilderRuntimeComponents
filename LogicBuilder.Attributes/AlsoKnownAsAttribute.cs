using System;

namespace LogicBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = false)]
    public class AlsoKnownAsAttribute : Attribute
    {
        public string AlsoKnownAs { get; private set; }
        /// <summary>
        /// Name field used in Logic Builder for function or variable configuration. Without the AlsoKnownAs attribute the default name is ClassName.MemberName.
        /// </summary>
        /// <param name="aka">Name field used in Logic Builder for a function or variable.</param>
        public AlsoKnownAsAttribute(string aka)
        {
            this.AlsoKnownAs = aka;
        }
    }
}
