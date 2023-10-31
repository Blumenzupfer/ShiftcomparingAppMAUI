using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.Models;
using ShiftComparingUI.Views.ComparingTables;

namespace ShiftComparingUI.ViewModels.ComparingTable;

public partial class ComparingTableViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private int _selectedIndex;
    [ObservableProperty] private ObservableCollection<ComparingTableModel> _comparingTables;

    public ComparingTableViewModel()
    {
        ComparingTables = new();
        foreach (var ct in ComparingTableDataAccess.GetAllComparingTables())
        {
            ComparingTables.Add(ct);
        }
    }

    [RelayCommand]
    async Task NavigateToAddComparingTableView()
    {
        await Shell.Current.GoToAsync(nameof(AddComparingTableView));
    }

    [RelayCommand]
    async Task NavigateToCreateAsCsvView(ComparingTableModel comparingTable)
    {
        IDictionary<string, object> navigationData = new Dictionary<string, object>()
        {
            { "comparingTable", comparingTable }
        };
        await Shell.Current.GoToAsync(nameof(CreateCsvView), navigationData);
    }

    [RelayCommand]
    private void DeleteComparingTable(ComparingTableModel comparingTable)
    {
        ComparingTableDataAccess.DeleteComparingTable(comparingTable.Id);
        ComparingTables.Remove(comparingTable);
    }

    [RelayCommand]
    private async Task EditComparingTable(ComparingTableModel comparingTable)
    {
        IDictionary<string, object> navigationData = new Dictionary<string, object>()
        {
            {"comparingTable", comparingTable}
        };
        await Shell.Current.GoToAsync(nameof(EditComparingTableView), navigationData);
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> navigationData)
    {
        var comparingTable = navigationData["comparingTable"] as ComparingTableModel;
        foreach (var ct in ComparingTables)
        {
            if (ct.Id == comparingTable?.Id)
            {
                ComparingTables[ComparingTables.IndexOf(ct)] = comparingTable;
                OnPropertyChanged(nameof(ComparingTables));
                return;
            }
        }
        ComparingTables.Add(comparingTable);
        OnPropertyChanged(nameof(ComparingTables));
    }
}