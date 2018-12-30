using System;
using System.Collections.Generic;

[assembly: CLSCompliant(true)]
namespace LogicBuilder.Forms.Parameters
{
    abstract public class BaseInputQuestionParameters
    {
        public BaseInputQuestionParameters(int Id, string VariableName, object QuestionData)
        {
            this.Id = Id;
            this.VariableName = VariableName;
            this.QuestionData = QuestionData;
        }

        public BaseInputQuestionParameters()
        {
        }

        public int Id { get; set; }
        public string VariableName { get; set; }
        public abstract string Type { get; }
        public object QuestionData { get; set; }
    }

    public class InputQuestionParameters<T> : BaseInputQuestionParameters
    {
        public InputQuestionParameters(int Id, string VariableName, T CurrentValue, object QuestionData)
            : base(Id, VariableName, QuestionData)
        {
            this.CurrentValue = CurrentValue;
        }

        public InputQuestionParameters()
        {
        }

        public T CurrentValue { get; set; }

        public override string Type 
            => typeof(T).IsGenericType
                ? typeof(T).AssemblyQualifiedName
                : typeof(T).FullName;
    }
}
