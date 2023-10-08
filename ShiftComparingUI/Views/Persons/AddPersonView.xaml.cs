using ShiftComparingUI.ViewModels;

namespace ShiftComparingUI.Views.Persons;

public partial class AddPersonView : ContentPage
{
    public AddPersonView(PersonsViewModel pvm)
    {
        InitializeComponent();
        BindingContext = pvm;
    }
}