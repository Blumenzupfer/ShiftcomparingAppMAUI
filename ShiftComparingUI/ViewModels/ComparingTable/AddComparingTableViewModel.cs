using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.Models;

namespace ShiftComparingUI.ViewModels.ComparingTable;

public partial class AddComparingTableViewModel : ObservableObject
{
    [ObservableProperty] private string _name;
    [ObservableProperty] private int _year;
    [ObservableProperty] private ObservableCollection<object> _selectedObjects;
    [ObservableProperty] private ObservableCollection<PersonModel> _selectedPersons;
    [ObservableProperty] private ObservableCollection<PersonModel> _listOfPersons;
    [ObservableProperty] private ObservableCollection<int> _yearList;

    public AddComparingTableViewModel()
    {
        ListOfPersons = new();
        SelectedObjects = new();
        SelectedPersons = new();
        YearList = new();
        AddYears();
        AddPersonsToList(ListOfPersons);
        Year = YearList.IndexOf(DateTime.Now.Year);
    }

    [RelayCommand]
    private async Task CreateNewComparingTable()
    {
        var comparingTable = new ComparingTableModel()
        {
            Name = Name,
            Year = Year,
            PersonsToBeCompared = SelectedPersons.ToList()
        };
        ComparingTableDataAccess.InsertComparingTable(comparingTable);
        comparingTable.Id = ComparingTableDataAccess.GetLastInsertedItemId();
        IDictionary<string, object> navigationData = new Dictionary<string, object>()
        {
            {"comparingTable", comparingTable}
        };
        await Shell.Current.GoToAsync("..", navigationData);

    }
    
    [RelayCommand]
    void SelectedPersonsChanged()
    {
        SelectedPersons.Clear();
        foreach (var p in SelectedObjects)
        {
            if (p is PersonModel person)
            {
                SelectedPersons.Add(person);
            }
        }
    }
    
    private void AddYears()
    {
        var startYear = DateTime.Now.Year - 100;
        Console.WriteLine(startYear);
        for (int i = startYear; i <= startYear + 200; i++)
        {
            YearList.Add(i);
        }
    }

    private void AddPersonsToList(ObservableCollection<PersonModel> listOfPersons)
    {
        foreach (var person in PersonDataAccess.GetAllPersons())
        {
            listOfPersons.Add(person);
        }
    }
    
}