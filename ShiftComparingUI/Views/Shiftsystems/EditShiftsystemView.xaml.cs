using ShiftComparingUI.ViewModels.Shiftsystems;

namespace ShiftComparingUI.Views.Shiftsystems;

public partial class EditShiftsystemView : ContentPage
{
    public EditShiftsystemView(EditShiftsystemViewModel esvm)
    {
        InitializeComponent();
        BindingContext = esvm;
    }
}