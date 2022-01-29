using LogicBuilder.Attributes;
using System.Linq;
using Xunit;

namespace Attributes.Tests
{
    public class AlsoKnownAsTest
    {
        [Fact]
        public void AlsoKnownAsWorksOnConstructors()
        {
            AlsoKnownAsAttribute attribute = (AlsoKnownAsAttribute)Helper.GetAttribute
            (
                typeof(SampleClass).GetConstructors().Single(),
                AttributeConstants.ALSOKNOWNASATTRIBUTE
            );

            Assert.Equal(TestConstants.SampleClass_1, attribute.AlsoKnownAs);
        }

        [Fact]
        public void AlsoKnownAsWorksOnMethods()
        {
            AlsoKnownAsAttribute attribute = (AlsoKnownAsAttribute)Helper.GetAttribute
            (
                typeof(SampleClass).GetMethod(TestConstants.MethodName),
                AttributeConstants.ALSOKNOWNASATTRIBUTE
            );

            Assert.Equal(TestConstants.My_Method, attribute.AlsoKnownAs);
        }

        [Fact]
        public void AlsoKnownAsWorksOnProperties()
        {
            AlsoKnownAsAttribute attribute = (AlsoKnownAsAttribute)Helper.GetAttribute
            (
                typeof(SampleClass).GetProperty(TestConstants.PropertyName), 
                AttributeConstants.ALSOKNOWNASATTRIBUTE
            );

            Assert.Equal(TestConstants.My_Property, attribute.AlsoKnownAs);
        }
    }
}
