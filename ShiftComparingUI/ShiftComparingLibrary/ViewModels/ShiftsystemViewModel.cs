using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ShiftComparingUI.ShiftComparingLibrary.Models;

namespace ShiftComparingUI.ShiftComparingLibrary.ViewModels;

public partial class ShiftsystemViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ShiftsystemModel> allShiftsystems;
}