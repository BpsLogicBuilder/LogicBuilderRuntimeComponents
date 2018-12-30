namespace LogicBuilder.Attributes
{
    public enum VariableControlType : short
    {
        SingleLineTextBox,
        MultipleLineTextBox,
        DropDown,
        TypeAutoComplete,
        DomainAutoComplete,
        PropertyInput,
        Form
    }

    public enum ParameterControlType : short
    {
        SingleLineTextBox,
        MultipleLineTextBox,
        DropDown,
        TypeAutoComplete,
        DomainAutoComplete,
        PropertyInput,
        ParameterSourcedPropertyInput,
        ParameterSourceOnly,
        Form
    }

    public enum ListControlType : short
    {
        ListForm,
        HashSetForm,
        Connectors
    }

    public enum FunctionGroup : short
    {
        Standard,
        DialogForm
    }
}
