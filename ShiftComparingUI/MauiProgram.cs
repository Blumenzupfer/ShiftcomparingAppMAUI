using Microsoft.Extensions.Logging;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.ShiftComparingLibrary.ViewModels;
using ShiftComparingUI.ViewModels;
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
        builder.Services.AddSingleton<ShiftsystemViewModel>();
        
        builder.Services.AddSingleton<AddShiftsystemView>();
        builder.Services.AddSingleton<EditShiftsystemView>();
        builder.Services.AddSingleton<AllShiftsystemsView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}