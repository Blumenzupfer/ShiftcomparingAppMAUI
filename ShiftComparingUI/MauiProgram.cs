using Microsoft.Extensions.Logging;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.ViewModels;
using ShiftComparingUI.ViewModels.Persons;
using ShiftComparingUI.ViewModels.ComparingTable;
using ShiftComparingUI.Views.ComparingTables;
using ShiftComparingUI.Views.Persons;
using ShiftComparingUI.Views.Shiftsystems;

namespace ShiftComparingUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        ShiftsystemDataAccess.CreateShiftsystemTable();
        PersonDataAccess.CreatePersonTable();
        ComparingTableDataAccess.CreateComparingTableTable();
        ComparingTableDataAccess.CreateReferenceTableTable();
        
        builder.Services.AddSingleton<ShiftsystemViewModel>();
        
        builder.Services.AddTransient<PersonsViewModel>();
        builder.Services.AddTransient<AddPersonViewModel>();
        
        builder.Services.AddTransient<ComparingTableViewModel>();
        builder.Services.AddTransient<AddComparingTableViewModel>();
        builder.Services.AddTransient<EditComparingTableViewModel>();
        
        builder.Services.AddSingleton<AllShiftsystemsView>();
        builder.Services.AddSingleton<AddShiftsystemView>();
        builder.Services.AddSingleton<EditShiftsystemView>();

        builder.Services.AddTransient<AllPersonsView>();
        builder.Services.AddTransient<AddPersonView>();

        builder.Services.AddTransient<AllComparingTablesView>();
        builder.Services.AddTransient<AddComparingTableView>();
        builder.Services.AddTransient<EditComparingTableView>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}