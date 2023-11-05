using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShiftComparingUI.HelperClasses;
using ShiftComparingUI.Models;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.Views.Shiftsystems;

namespace ShiftComparingUI.ViewModels;

public partial class ShiftsystemViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private string _name = String.Empty;
    [ObservableProperty] private string _description = String.Empty;
    [ObservableProperty] private ObservableCollection<string> _shiftpattern = new();
    [ObservableProperty] private int _numberOfShiftgroups;
    [ObservableProperty] private ObservableCollection<ShiftgroupDatetimes> _shiftgroupStartdates = new();
    [ObservableProperty] private bool _isNotValidNumberForShiftsystem;
    [ObservableProperty] private bool _isEmptyName;
    [ObservableProperty] private bool _isEmptyDescription;
    [ObservableProperty] private bool _isEmptyShiftpattern;
    [ObservableProperty] private ObservableCollection<ShiftsystemModel> _listOfShiftsystems = new();
    private int _shiftsystemId;
    
    public ShiftsystemViewModel()
    {
        IsNotValidNumberForShiftsystem = false;
        foreach (var shiftsystem in ShiftsystemDataAccess.GetAllShiftsystems())
        {
            ListOfShiftsystems.Add(shiftsystem);
        }
    }
    
    [RelayCommand]
    async Task NavigateToAddPage()
    {
        await Shell.Current.GoToAsync($"{nameof(AddShiftsystemView)}");
    }

    void ClearDataFields()
    {
        Name = Description = string.Empty;
        Shiftpattern.Clear();
        ShiftgroupStartdates.Clear();
        NumberOfShiftgroups = 0;
    }

    [RelayCommand]
    void DeleteShiftsystem(ShiftsystemModel shiftsystem)
    {
        if (ShiftsystemDataAccess.DeleteShiftsystem(shiftsystem.Id))
        {
            ListOfShiftsystems.Remove(shiftsystem);
        }
    }
    
    // Edit Page
    [RelayCommand]
    async Task NavigateToEditPage(ShiftsystemModel shiftsystem)
    {
        _shiftsystemId = shiftsystem.Id;
        Name = shiftsystem.Name;
        Description = shiftsystem.Description;
        foreach (var content in ShiftsystemModel.StringToList(shiftsystem.Shiftpattern))
        {
            Shiftpattern.Add(content);
        }
        foreach (var content in ShiftsystemModel.ShiftgroupStartdatesToList(shiftsystem.ShiftgroupStartdates))
        {
            ShiftgroupStartdates.Add(content);
        }
        NumberOfShiftgroups = ShiftgroupStartdates.Count;
        await Shell.Current.GoToAsync(nameof(EditShiftsystemView));
    }
    
    [RelayCommand]
    async Task EditShiftsystem()
    {
        var updatedShiftsystem = new ShiftsystemModel()
        {
            Id = _shiftsystemId,
            Name = Name,
            Description = Description,
            Shiftpattern = ShiftsystemModel.ListToString(Shiftpattern.ToList()),
            ShiftgroupStartdates = ShiftsystemModel.ShiftgroupStartdatesToString(ShiftgroupStartdates.ToList())
        };
        
        if (!ShiftsystemDataAccess.UpdateShiftsystem(updatedShiftsystem)) return;
        
        var shiftsystem = ListOfShiftsystems.Single(x => x.Id == _shiftsystemId);
        ListOfShiftsystems.Remove(shiftsystem);
        ListOfShiftsystems.Add(updatedShiftsystem);
        
        ClearDataFields();

        await Shell.Current.GoToAsync("..");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> navigationData)
    {
        var shiftsystem = navigationData["shiftsystem"] as ShiftsystemModel;
        if (ListOfShiftsystems.Contains(shiftsystem))
        {
            return;
        }

        ListOfShiftsystems.Add(shiftsystem);
        OnPropertyChanged(nameof(ListOfShiftsystems));
    }
}