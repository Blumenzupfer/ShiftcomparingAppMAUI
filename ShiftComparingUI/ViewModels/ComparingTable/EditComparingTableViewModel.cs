using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.Models;
using ShiftComparingUI.Views.ComparingTables;

namespace ShiftComparingUI.ViewModels.ComparingTable;

public partial class EditComparingTableViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private string _name;
    [ObservableProperty] private int _year;
    [ObservableProperty] private PersonModel _selectedPerson;
    [ObservableProperty] private PersonModel _unselectedPerson;
    [ObservableProperty] private ObservableCollection<PersonModel> _selectedPersons;
    [ObservableProperty] private ObservableCollection<PersonModel> _listOfPersons;
    [ObservableProperty] private ObservableCollection<int> _yearList;
    private int _comparingTableId;
    private List<PersonModel> _oldSelectedPersons;
    private List<PersonModel> _newSelectedPersons;

    public EditComparingTableViewModel()
    {
        ListOfPersons = new();
        SelectedPersons = new();
        _oldSelectedPersons = new();
        _newSelectedPersons = new();
        YearList = new();
        AddYears();
    }
    
    [RelayCommand]
    void SelectedPersonChanged()
    {
        ListOfPersons.Remove(SelectedPerson);
        SelectedPersons.Add(SelectedPerson);
    }
    
    [RelayCommand]
    void UnselectedPersonChanged()
    {
        SelectedPersons.Remove(UnselectedPerson);
        ListOfPersons.Add(UnselectedPerson);
    }

    [RelayCommand]
    async Task UpdateComparingTable()
    {
        Console.WriteLine("Run First");
        FilterAddableAndDeletablePersons();
        Console.WriteLine("Run Second");
        var comparingTable = new ComparingTableModel()
        {
            Id = _comparingTableId,
            Name = Name,
            Year = Year,
            PersonsToBeCompared = SelectedPersons.ToList()
        };
        Console.WriteLine("Run Third");
        ComparingTableDataAccess.UpdateComparingTable(comparingTable, _oldSelectedPersons, _newSelectedPersons);
        Console.WriteLine("Run Last");
        IDictionary<string, object> navigationData = new Dictionary<string, object>()
        {
            { "comparingTable", comparingTable }
        };
        await Shell.Current.GoToAsync("..", navigationData);
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

    private void CreateListOfPersons()
    {
        var unfilteredList = PersonDataAccess.GetAllPersons();
        var sameIds = new List<int>();
        sameIds = unfilteredList.Select(p => p.Id).Intersect(SelectedPersons.Select(sp => sp.Id)).ToList();
        unfilteredList.RemoveAll(a => sameIds.Contains(a.Id));
        foreach (var person in unfilteredList)
        {
            ListOfPersons.Add(person);
        }
    }

    private void FilterAddableAndDeletablePersons()
    {
        foreach (var person in SelectedPersons)
        {
            if (_oldSelectedPersons.Contains(person))
            {
                _oldSelectedPersons.Remove(person); // deletes Person if they are also in the new selection
            }
            else
            {
                _newSelectedPersons.Add(person); // adds Persons who weren't in the old selection
            }
        }
        // OldSelectedPersons need to be deleted in the database
        // newSelectedPersons need to be added to the database
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> navigationData)
    {
        if (navigationData["comparingTable"] is ComparingTableModel comparingTable)
        {
            _comparingTableId = comparingTable.Id;
            Name = comparingTable.Name;
            Year = comparingTable.Year;
            foreach (var person in comparingTable.PersonsToBeCompared)
            {
                SelectedPersons.Add(person);
                _oldSelectedPersons.Add(person);
            }
            CreateListOfPersons();
        }
        OnPropertyChanged(nameof(Name));
    }
}