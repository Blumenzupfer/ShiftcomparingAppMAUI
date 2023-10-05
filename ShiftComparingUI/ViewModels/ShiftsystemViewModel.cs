using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShiftComparingUI.HelperClasses;
using ShiftComparingUI.Models;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.Views.Shiftsystems;

namespace ShiftComparingUI.ViewModels;

public partial class ShiftsystemViewModel : ObservableObject
{
    [ObservableProperty] private string _name = String.Empty;
    [ObservableProperty] private string _description = String.Empty;
    [ObservableProperty] private ObservableCollection<string> _shiftpattern;
    [ObservableProperty] private int _numberOfShiftgroups;
    [ObservableProperty] private ObservableCollection<ShiftgroupDatetimes> _shiftgroupStartdates;
    [ObservableProperty] private bool _isNotValidNumberForShiftsystem;
    [ObservableProperty] private bool _isEmptyName;
    [ObservableProperty] private bool _isEmptyDescription;
    [ObservableProperty] private bool _isEmptyShiftpattern;
    [ObservableProperty] private ObservableCollection<ShiftsystemModel> _listOfShiftsystems;
    private int _shiftsystemId;

    public ShiftsystemViewModel()
    {
        Shiftpattern = new ObservableCollection<string>();
        ShiftgroupStartdates = new ObservableCollection<ShiftgroupDatetimes>();
        IsNotValidNumberForShiftsystem = false;
        ListOfShiftsystems = new ObservableCollection<ShiftsystemModel>();
        foreach (var shiftsystem in ShiftsystemDataAccess.GetAllShiftsystems())
        {
            ListOfShiftsystems.Add(shiftsystem);
        }
    }

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
    void CheckShiftgroupsWithShiftsystem()
    {
        IsNotValidNumberForShiftsystem = false;
        if (ShiftgroupStartdates.Count != 0)
        {
            ShiftgroupStartdates.Clear();
        }

        if (NumberOfShiftgroups == 0)
        {
            IsNotValidNumberForShiftsystem = true;
        }
        else if (Shiftpattern.Count % NumberOfShiftgroups == 0
            && NumberOfShiftgroups is > 0 and <= 5)
        {
            CreateShiftgroupStartdates();
        }
        else
        {
            IsNotValidNumberForShiftsystem = true;
        }
    }

    void CreateShiftgroupStartdates()
    {
        for (int i = 1; i <= NumberOfShiftgroups; i++)
        {
            ShiftgroupStartdates.Add(new ShiftgroupDatetimes($"Shiftgroup {i}: "));
        }
    }
    
    [RelayCommand]
    async Task NavigateToAddPage()
    {
        await Shell.Current.GoToAsync($"{nameof(AddShiftsystemView)}");
    }

    [RelayCommand]
    void CreateShiftsystem()
    {
        IsEmptyName = Name == string.Empty;
        IsEmptyDescription = Description == string.Empty;
        IsEmptyShiftpattern = Shiftpattern.Count == 0;
        if (NumberOfShiftgroups == 0)
        {
            IsNotValidNumberForShiftsystem = true;
        }
        else if (Shiftpattern.Count % NumberOfShiftgroups == 0 && NumberOfShiftgroups is > 0 and <= 5)
        {
            IsNotValidNumberForShiftsystem = false;
        }
        else
        {
            IsNotValidNumberForShiftsystem = true;
        }

        if (Name == String.Empty
            || Description == String.Empty
            || Shiftpattern.Count == 0
            || Shiftpattern.Count % NumberOfShiftgroups != 0
            || NumberOfShiftgroups is <= 0 or > 5) return;
        
        ShiftsystemModel newShiftsystem = new ShiftsystemModel()
        {
            Name = Name,
            Description = Description,
            Shiftpattern = ShiftsystemModel.ListToString(Shiftpattern.ToList()),
            ShiftgroupStartdates = ShiftsystemModel.ShiftgroupStartdatesToString(ShiftgroupStartdates.ToList())
        };
        
        int id = ShiftsystemDataAccess.AddNewShiftsystem(newShiftsystem);
        newShiftsystem.Id = id;
        ListOfShiftsystems.Add(newShiftsystem);
        
        ClearDataFields();
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

    [RelayCommand]
    void DeleteShiftpatternEntry(string entry)
    {
        Shiftpattern.Remove(entry);
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

}