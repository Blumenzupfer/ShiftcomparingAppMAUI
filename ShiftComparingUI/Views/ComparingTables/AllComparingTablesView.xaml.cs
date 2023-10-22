using ShiftComparingUI.ViewModels.ComparingTable;

namespace ShiftComparingUI.Views.ComparingTables;

public partial class AllComparingTablesView : ContentPage
{
    public AllComparingTablesView(ComparingTableViewModel ctvm)
    {
        InitializeComponent();
        BindingContext = ctvm;
    }
}