namespace E220_900T225_ConfigTool.ViewModels
{
    public sealed class ComboBoxViewModel { 
        public ComboBoxViewModel(int value, string displayValue)
        { 
            Value = value;
            DisplayValue = displayValue;
        } 
        
        public int Value { get; } 
        public string DisplayValue { get; } 
    }
}
