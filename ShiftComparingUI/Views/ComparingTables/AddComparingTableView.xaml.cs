using ShiftComparingUI.ViewModels.ComparingTable;

namespace ShiftComparingUI.Views.ComparingTables;

public partial class AddComparingTableView : ContentPage
{
    public AddComparingTableView(AddComparingTableViewModel actvm)
    {
        InitializeComponent();
        BindingContext = actvm;
    }
}