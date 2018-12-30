using System;

namespace LogicBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class CommentsAttribute : Attribute
    {
        public string Comments { get; private set; }
        /// <summary>
        /// Comments about the variable.
        /// </summary>
        /// <param name="comments">Comments about the variable.</param>
        public CommentsAttribute(string comments)
        {
            this.Comments = comments;
        }
    }
}
