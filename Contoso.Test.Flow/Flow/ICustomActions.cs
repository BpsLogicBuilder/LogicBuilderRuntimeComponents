using LogicBuilder.Attributes;

namespace Contoso.Test.Flow
{
    public interface ICustomActions
    {
        [AlsoKnownAs("WriteToLog")]
        void WriteToLog(string message);
    }
}
