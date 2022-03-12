using LogicBuilder.Attributes;

namespace Attributes.Tests
{
    internal class SampleClass
    {
        [AlsoKnownAs(TestConstants.SampleClass_1)]
        [Summary(TestConstants.SummaryText)]
        public SampleClass
        (
            int myProperty
        )
        {
            MyProperty = myProperty;
        }

        [AlsoKnownAs(TestConstants.My_Property)]
        public int MyProperty { get; set; }


        [AlsoKnownAs(TestConstants.My_Method)]
        [Summary(TestConstants.SummaryText)]
        public void MyMethod()
        {
        }
    }


}
