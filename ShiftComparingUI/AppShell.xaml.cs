using ShiftComparingUI.Views.ComparingTables;
using ShiftComparingUI.Views.Persons;
using ShiftComparingUI.Views.Shiftsystems;

namespace ShiftComparingUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(AddShiftsystemView), typeof(AddShiftsystemView));
        Routing.RegisterRoute(nameof(EditShiftsystemView), typeof(EditShiftsystemView));

        Routing.RegisterRoute(nameof(AddPersonView), typeof(AddPersonView));
        
        Routing.RegisterRoute(nameof(AddComparingTableView), typeof(AddComparingTableView));
        Routing.RegisterRoute(nameof(EditComparingTableView), typeof(EditComparingTableView));
    }
}