using System;

namespace LogicBuilder.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DomainAttribute : Attribute
    {
        public string DomainList { get; private set; }
        /// <summary>
        /// Domain Attribute
        /// </summary>
        /// <param name="domainList">Comma delimited list of items</param>
        public DomainAttribute(string domainList)
        {
            this.DomainList = domainList;
        }
    }
}
