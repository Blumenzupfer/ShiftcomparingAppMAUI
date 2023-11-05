using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.HelperClasses;
using ShiftComparingUI.Models;

namespace ShiftComparingUI.ViewModels.Shiftsystems;

public partial class EditShiftsystemViewModel : ObservableObject, IQueryAttributable
{
    private int _shiftsystemId;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private int _numberOfShiftgroups;
    [ObservableProperty] private ObservableCollection<string> _shiftpattern = new();
    [ObservableProperty] private ObservableCollection<ShiftgroupDatetimes> _shiftgroupStartdates = new();
    [ObservableProperty] private bool _isNotValidNumberForShiftsystem;
    [ObservableProperty] private bool _isEmptyName;
    [ObservableProperty] private bool _isEmptyDescription;
    [ObservableProperty] private bool _isEmptyShiftpattern;

    [RelayCommand]
    void AddF12()
    {
        Shiftpattern.Add("F12");
    }
    [RelayCommand]
    void AddN12()
    {
        Shiftpattern.Add("N12");
    }
    [RelayCommand]
    void AddF()
    {
        Shiftpattern.Add("F");
    }
    [RelayCommand]
    void AddS()
    {
        Shiftpattern.Add("S");
    }
    [RelayCommand]
    void AddN()
    {
        Shiftpattern.Add("N");
    }
    [RelayCommand]
    void AddMinus()
    {
        Shiftpattern.Add("-");
    }
    
    [RelayCommand]
    void DeleteShiftpatternEntry(string entry)
    {
        Shiftpattern.Remove(entry);
    }
    
    [RelayCommand]
    private void CheckShiftgroupsWithShiftsystem()
    {
        ClearShiftgroupStartdatesIfNotEmpty();
        IsNotValidNumberForShiftsystem = ValidateShiftgroupNumber();
        if (!IsNotValidNumberForShiftsystem)
        {
            CreateShiftgroupStartdates();
        }
    }
    
    [RelayCommand]
    private async Task EditShiftsystem()
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

        IDictionary<string, object> navigationData = new Dictionary<string, object>()
        {
            { "shiftsystem", updatedShiftsystem }
        };
        ClearDataFields();

        await Shell.Current.GoToAsync("..", navigationData);
    }
    
    private void ClearShiftgroupStartdatesIfNotEmpty()
    {
        if (ShiftgroupStartdates.Count != 0)
        {
            ShiftgroupStartdates.Clear();
        }
    }
    
    private bool ValidateShiftgroupNumber()
    {
        if (NumberOfShiftgroups == 0)
        {
            return true;
        }
        return Shiftpattern.Count % NumberOfShiftgroups != 0
               || NumberOfShiftgroups is <= 0 or > 5;
    }
    
    private void CreateShiftgroupStartdates()
    {
        var shiftgroupLetters = new List<string>() { "A", "B", "C", "D", "E" };
        for (var i = 1; i <= NumberOfShiftgroups; i++)
        {
            ShiftgroupStartdates.Add(new ShiftgroupDatetimes($"{shiftgroupLetters[i-1]}-Shift"));
        }
    }
    
    private void ClearDataFields()
    {
        Name = Description = string.Empty;
        Shiftpattern.Clear();
        ShiftgroupStartdates.Clear();
        NumberOfShiftgroups = 0;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> navigationData)
    {
        if (navigationData["shiftsystem"] is ShiftsystemModel shiftsystem)
        {
            _shiftsystemId = shiftsystem.Id;
            Name = shiftsystem.Name;
            Description = shiftsystem.Description;
            Shiftpattern = new ObservableCollection<string>(ShiftsystemModel.StringToList(shiftsystem.Shiftpattern));
            ShiftgroupStartdates =
                new ObservableCollection<ShiftgroupDatetimes>(
                    ShiftsystemModel.ShiftgroupStartdatesToList(shiftsystem.ShiftgroupStartdates));
            NumberOfShiftgroups = ShiftgroupStartdates.Count;
        }
        
    }
}