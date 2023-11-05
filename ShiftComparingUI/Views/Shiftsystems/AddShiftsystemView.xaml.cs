using ShiftComparingUI.ViewModels.Shiftsystems;

namespace ShiftComparingUI.Views.Shiftsystems;

public partial class AddShiftsystemView : ContentPage
{
    public AddShiftsystemView(AddShiftsystemViewModel avm)
    {
        InitializeComponent();
        BindingContext = avm;
    }
}