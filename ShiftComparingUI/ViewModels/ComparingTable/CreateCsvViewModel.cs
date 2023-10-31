using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShiftComparingUI.Models;
using ShiftComparingUI.Services;

namespace ShiftComparingUI.ViewModels.ComparingTable;

public partial class CreateCsvViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private string _name;
    [ObservableProperty] private int _year;
    [ObservableProperty] private ObservableCollection<PersonModel> _personsToBeCompared = new();
    [ObservableProperty] private ObservableCollection<string> _monthNames = new();
    [ObservableProperty] private string _worksheetName;
    [ObservableProperty] private string _directory;
    private string _directoryPath;

    public CreateCsvViewModel()
    {
        for (int i = 1; i < 12; i++)
        {
            MonthNames.Add(new DateTime(2022, i, 1).ToString("MMMM"));
        }
    }

    [RelayCommand]
    private async Task AddFolderPath()
    {
        var result = await FolderPicker.PickAsync(default);
        if (result.Folder != null)
        {
            _directoryPath = result.Folder.Path;
            Directory = result.Folder.Name;
        }
    }

    [RelayCommand]
    private void CreateCsvFile()
    {
        var comparingTable = new ComparingTableModel()
        {
            Name = Name,
            Year = Year,
            PersonsToBeCompared = PersonsToBeCompared.ToList()
        };
        var excelService = new ExcelService(comparingTable, WorksheetName, _directoryPath);
        excelService.CreateComparingTableInCsvFile();
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> navigationData)
    {
        if (navigationData["comparingTable"] is ComparingTableModel comparingTable)
        {
            Name = comparingTable.Name;
            Year = comparingTable.Year;
            foreach (var person in comparingTable.PersonsToBeCompared)
            {
                PersonsToBeCompared.Add(person);
            }
        }
    }
}