using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShiftComparingUI.ShiftComparingLibrary.HelperClasses;
using ShiftComparingUI.ShiftComparingLibrary.Models;
using Newtonsoft.Json;

namespace ShiftComparingUI.ShiftComparingLibrary.ViewModels;

public partial class AddShiftsystemViewModel : ObservableObject
{
    [ObservableProperty]
    private string name;
    [ObservableProperty]
    private string description;
    [ObservableProperty]
    private ObservableCollection<string> shiftpattern;
    [ObservableProperty]
    private int numberOfShiftgroups;
    [ObservableProperty] 
    private ObservableCollection<ShiftgroupDatetimes> shiftgroupStartdates;
    [ObservableProperty] 
    private bool _isNotValidNumberForShiftsystem;
    [ObservableProperty]
    private bool isEmptyName;
    [ObservableProperty]
    private bool isEmptyDescription;
    [ObservableProperty]
    private bool isEmptyShiftpattern;

    public AddShiftsystemViewModel()
    {
        Shiftpattern = new ObservableCollection<string>();
        ShiftgroupStartdates = new ObservableCollection<ShiftgroupDatetimes>();
        IsNotValidNumberForShiftsystem = false;
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

        if (Shiftpattern.Count % NumberOfShiftgroups == 0
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

    void CreateShiftsystem()
    {
        if(Name == "")
        {
            IsEmptyName = true;
        }
        if(Description == "")
        {
            IsEmptyDescription = true;
        }
        if(Shiftpattern.Count == 0)
        {
            IsEmptyShiftpattern = true;
        }
        if(Shiftpattern.Count % NumberOfShiftgroups != 0
            && NumberOfShiftgroups is not > 0 and <= 5)
        {
            IsNotValidNumberForShiftsystem = true;
        }
        if(Name != ""
            && Description != ""
            && Shiftpattern.Count != 0
            && Shiftpattern.Count % NumberOfShiftgroups == 0
            && NumberOfShiftgroups is > 0 and <= 5)
        {
            ShiftsystemModel newShiftsystem = new ShiftsystemModel() { Name = Name,
                Description = Description,
                Shiftpattern = ShiftsystemModel.ListToString(Shiftpattern.ToList()),
                ShiftgroupStartdates = ShiftsystemModel.ShiftgroupStartdatesToString(ShiftgroupStartdates.ToList())
                };
        }
        

    }
    

}