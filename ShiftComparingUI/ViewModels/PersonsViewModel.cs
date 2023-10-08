using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.HelperClasses;
using ShiftComparingUI.Models;
using ShiftComparingUI.Views.Persons;

namespace ShiftComparingUI.ViewModels;

public partial class PersonsViewModel : ObservableObject
{
    [ObservableProperty] private string _name;
    [ObservableProperty] private string _tableName;
    [ObservableProperty] private ShiftsystemModel _selectedShiftsystem;
    [ObservableProperty] private int _shiftgroupNumber;
    [ObservableProperty] private ObservableCollection<ShiftsystemModel> _listOfShiftsystems;
    [ObservableProperty] private ObservableCollection<ShiftgroupDatetimes> _shiftgroupList;
    [ObservableProperty] private ShiftgroupDatetimes _selectedShiftgroup;
    [ObservableProperty] private ObservableCollection<PersonModel> _listOfPersons;

    public PersonsViewModel()
    {
        ShiftgroupList = new ObservableCollection<ShiftgroupDatetimes>();
        ListOfShiftsystems = new ObservableCollection<ShiftsystemModel>();
        ListOfPersons = new ObservableCollection<PersonModel>();
        foreach (var shiftsystem in ShiftsystemDataAccess.GetAllShiftsystems())
        {
            ListOfShiftsystems.Add(shiftsystem);
        }
        foreach (var person in PersonDataAccess.GetAllPersons())
        {
            ListOfPersons.Add(person);
        }
    }
    
    [RelayCommand]
    async Task NavigateToAddPersonPage()
    {
        await Shell.Current.GoToAsync(nameof(AddPersonView));
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
    void CreatePerson()
    {
        var newPerson = new PersonModel()
        {
            Name = Name,
            TableName = TableName,
            ShiftsystemId = SelectedShiftsystem.Id,
            Shiftgroup = ShiftgroupNumber
        };
        if (PersonDataAccess.InsertPerson(newPerson))
        {
            newPerson = PersonDataAccess.GetAllPersons().Last();
            ListOfPersons.Add(newPerson);
        }
    }

    [RelayCommand]
    void DeletePerson(PersonModel person)
    {
        if (PersonDataAccess.DeletePerson(person.Id))
        {
            ListOfPersons.Remove(person);
        }
    }
}