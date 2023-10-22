using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.Models;
using ShiftComparingUI.Views.Persons;

namespace ShiftComparingUI.ViewModels.Persons;

public partial class PersonsViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] private ObservableCollection<PersonModel> _listOfPersons;

    public PersonsViewModel()
    {
        ListOfPersons = new ObservableCollection<PersonModel>();
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
    void DeletePerson(PersonModel person)
    {
        if (PersonDataAccess.DeletePerson(person.Id))
        {
            ListOfPersons.Remove(person);
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> navigationData)
    {
        var person = navigationData["person"] as PersonModel;
        if (ListOfPersons.Contains(person))
        {
            return;
        }
        ListOfPersons.Add(person);
        OnPropertyChanged(nameof(ListOfPersons));
    }
}