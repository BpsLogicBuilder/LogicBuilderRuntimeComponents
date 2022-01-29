using LogicBuilder.Attributes;
using System.Linq;
using Xunit;

namespace Attributes.Tests
{
    public class SummaryTest
    {
        [Fact]
        public void SummaryWorksOnConstructors()
        {
            SummaryAttribute attribute = (SummaryAttribute)Helper.GetAttribute
            (
                typeof(SampleClass).GetConstructors().Single(),
                AttributeConstants.SUMMARYATTRIBUTE
            );

            Assert.Equal(TestConstants.SummaryText, attribute.Summary);
        }

        [Fact]
        public void SummaryWorksOnMethods()
        {
            SummaryAttribute attribute = (SummaryAttribute)Helper.GetAttribute
            (
                typeof(SampleClass).GetMethod(TestConstants.MethodName),
                AttributeConstants.SUMMARYATTRIBUTE
            );

            Assert.Equal(TestConstants.SummaryText, attribute.Summary);
        }
    }
}
