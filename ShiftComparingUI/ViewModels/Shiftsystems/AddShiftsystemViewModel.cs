using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.HelperClasses;
using ShiftComparingUI.Models;

namespace ShiftComparingUI.ViewModels.Shiftsystems;

public partial class AddShiftsystemViewModel : ObservableObject
{
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private int _numberOfShiftgroups;
    [ObservableProperty] private bool _isNotValidNumberForShiftsystem;
    [ObservableProperty] private bool _isEmptyName;
    [ObservableProperty] private bool _isEmptyDescription;
    [ObservableProperty] private bool _isEmptyShiftpattern;
    [ObservableProperty] private ObservableCollection<string> _shiftpattern = new();
    [ObservableProperty] private ObservableCollection<ShiftgroupDatetimes> _shiftgroupStartdates = new();

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
    private async Task CreateShiftsystem()
    {
        ValidateInputFieldVariables();

        if (IsDataInvalid()) return;
        
        var newShiftsystem = new ShiftsystemModel()
        {
            Name = Name,
            Description = Description,
            Shiftpattern = ShiftsystemModel.ListToString(Shiftpattern.ToList()),
            ShiftgroupStartdates = ShiftsystemModel.ShiftgroupStartdatesToString(ShiftgroupStartdates.ToList())
        };
        
        var id = ShiftsystemDataAccess.AddNewShiftsystem(newShiftsystem);
        newShiftsystem.Id = id;
        IDictionary<string, object> navigationData = new Dictionary<string, object>()
        {
            { "shiftsystem", newShiftsystem }
        };
        await Shell.Current.GoToAsync("..", navigationData);
        ClearDataFields();
    }
    
    [RelayCommand]
    void DeleteShiftpatternEntry(string entry)
    {
        Shiftpattern.Remove(entry);
    }

    private bool IsDataInvalid()
    {
        return Name == string.Empty
               || Description == string.Empty
               || Shiftpattern.Count == 0
               || Shiftpattern.Count % NumberOfShiftgroups != 0
               || NumberOfShiftgroups is <= 0 or > 5;
    }
    
    private void ValidateInputFieldVariables()
    {
        IsEmptyName = Name == string.Empty;
        IsEmptyDescription = Description == string.Empty;
        IsEmptyShiftpattern = Shiftpattern.Count == 0;
        IsNotValidNumberForShiftsystem = ValidateShiftgroupNumber();
    }
    
    private void ClearDataFields()
    {
        Name = Description = string.Empty;
        Shiftpattern.Clear();
        ShiftgroupStartdates.Clear();
        NumberOfShiftgroups = 0;
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
}