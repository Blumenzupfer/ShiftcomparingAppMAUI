using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShiftComparingUI.Models;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.Views.Shiftsystems;

namespace ShiftComparingUI.ViewModels;

public partial class ShiftsystemViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private ObservableCollection<ShiftsystemModel> _listOfShiftsystems = new();
    
    public ShiftsystemViewModel()
    {
        ListOfShiftsystems = new ObservableCollection<ShiftsystemModel>(ShiftsystemDataAccess.GetAllShiftsystems());
    }
    
    [RelayCommand]
    async Task NavigateToAddPage()
    {
        await Shell.Current.GoToAsync($"{nameof(AddShiftsystemView)}");
    }

    [RelayCommand]
    void DeleteShiftsystem(ShiftsystemModel shiftsystem)
    {
        if (ShiftsystemDataAccess.DeleteShiftsystem(shiftsystem.Id))
        {
            ListOfShiftsystems.Remove(shiftsystem);
        }
    }
    
    [RelayCommand]
    async Task NavigateToEditPage(ShiftsystemModel shiftsystem)
    {
        IDictionary<string, object> navigationData = new Dictionary<string, object>()
        {
            { "shiftsystem", shiftsystem }
        };
        await Shell.Current.GoToAsync(nameof(EditShiftsystemView), navigationData);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> navigationData)
    {
        var shiftsystem = navigationData["shiftsystem"] as ShiftsystemModel;

        if (ListOfShiftsystems.Any(s => s.Id == shiftsystem?.Id))
        {
            var oldShiftsystem = ListOfShiftsystems.Single(s => s.Id == shiftsystem?.Id);
            var shiftsystemIndex = ListOfShiftsystems.IndexOf(oldShiftsystem);
            ListOfShiftsystems[shiftsystemIndex] = shiftsystem;
            OnPropertyChanged(nameof(ListOfShiftsystems));
            return;
        }

        ListOfShiftsystems.Add(shiftsystem);
        OnPropertyChanged(nameof(ListOfShiftsystems));
    }
}