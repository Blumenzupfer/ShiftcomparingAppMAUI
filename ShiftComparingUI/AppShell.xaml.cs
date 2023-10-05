using ShiftComparingUI.Views.Shiftsystems;

namespace ShiftComparingUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(AddShiftsystemView), typeof(AddShiftsystemView));
        Routing.RegisterRoute(nameof(EditShiftsystemView), typeof(EditShiftsystemView));
        Routing.RegisterRoute(nameof(AllShiftsystemsView), typeof(AllShiftsystemsView));
    }
}