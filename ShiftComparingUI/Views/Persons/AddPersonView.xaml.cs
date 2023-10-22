using ShiftComparingUI.ViewModels.Persons;

namespace ShiftComparingUI.Views.Persons;

public partial class AddPersonView : ContentPage
{
    public AddPersonView(AddPersonViewModel pvm)
    {
        InitializeComponent();
        BindingContext = pvm;
    }
}