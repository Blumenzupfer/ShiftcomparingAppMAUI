using ShiftComparingUI.ViewModels;

namespace ShiftComparingUI.Views.Shiftsystems;

public partial class AddShiftsystemView : ContentPage
{
    public AddShiftsystemView(ShiftsystemViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}