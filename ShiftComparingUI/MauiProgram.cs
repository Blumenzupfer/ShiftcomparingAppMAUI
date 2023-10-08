using Microsoft.Extensions.Logging;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.ViewModels;
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
        
        builder.Services.AddSingleton<ShiftsystemViewModel>();
        builder.Services.AddSingleton<PersonsViewModel>();
        
        builder.Services.AddSingleton<AddShiftsystemView>();
        builder.Services.AddSingleton<EditShiftsystemView>();
        builder.Services.AddSingleton<AllShiftsystemsView>();

        builder.Services.AddSingleton<AddPersonView>();
        builder.Services.AddSingleton<AllPersonsView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}