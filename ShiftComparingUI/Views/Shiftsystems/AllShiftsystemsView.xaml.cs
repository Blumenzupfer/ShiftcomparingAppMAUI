
using ShiftComparingUI.ViewModels;

namespace ShiftComparingUI.Views.Shiftsystems;

public partial class AllShiftsystemsView : ContentPage
{
    public AllShiftsystemsView(ShiftsystemViewModel svm)
    {
        InitializeComponent();
        BindingContext = svm;
    }
}