using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.HelperClasses;
using ShiftComparingUI.Models;

namespace ShiftComparingUI.ViewModels.Persons;

public partial class AddPersonViewModel : ObservableObject
{
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _tableName;
    [ObservableProperty] private ShiftsystemModel _selectedShiftsystem;
    [ObservableProperty] private int _shiftgroupNumber;
    [ObservableProperty] private ObservableCollection<ShiftsystemModel> _listOfShiftsystems;
    [ObservableProperty] private ObservableCollection<ShiftgroupDatetimes> _shiftgroupList;
    [ObservableProperty] private ShiftgroupDatetimes _selectedShiftgroup;

    public AddPersonViewModel()
    {
        ShiftgroupList = new();
        ListOfShiftsystems = new ();
        foreach (var shiftsystem in ShiftsystemDataAccess.GetAllShiftsystems())
        {
            ListOfShiftsystems.Add(shiftsystem);
        }
    }
    
    [RelayCommand]
    void SelectShiftsystem()
    {
        foreach (var shiftgroup in ShiftsystemModel.ShiftgroupStartdatesToList(SelectedShiftsystem.ShiftgroupStartdates))
        {
            ShiftgroupList.Add(shiftgroup);
        }
    }
    
    [RelayCommand]
    void SelectShiftgroup()
    {
        ShiftgroupNumber = ShiftgroupList.IndexOf(SelectedShiftgroup);
    }
    
    [RelayCommand]
    async Task CreatePerson()
    {
        var newPerson = new PersonModel()
        {
            Name = Name,
            TableName = TableName,
            ShiftsystemId = SelectedShiftsystem.Id,
            Shiftgroup = ShiftgroupNumber
        };
        PersonDataAccess.InsertPerson(newPerson);
        newPerson = PersonDataAccess.GetAllPersons().Last();
        IDictionary<string, object> navigationData = new Dictionary<string, object>()
        {
            { "person", newPerson }
        };
        Console.WriteLine("executed four");
        await Shell.Current.GoToAsync("..", navigationData);
    }

    void ClearDataFields()
    {
        Name = string.Empty;
        TableName = string.Empty;
        SelectedShiftsystem = new();
        ShiftgroupList.Clear();
    }
}