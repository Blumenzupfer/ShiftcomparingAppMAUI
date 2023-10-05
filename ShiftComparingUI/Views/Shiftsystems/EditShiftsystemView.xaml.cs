using ShiftComparingUI.ViewModels;

namespace ShiftComparingUI.Views.Shiftsystems;

public partial class EditShiftsystemView : ContentPage
{
    public EditShiftsystemView(ShiftsystemViewModel svm)
    {
        InitializeComponent();
        BindingContext = svm;
    }
}