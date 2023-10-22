using CommunityToolkit.Mvvm.Messaging.Messages;
using ShiftComparingUI.Models;
using ShiftComparingUI.ViewModels;

namespace ShiftComparingUI.Messages;

public class ViewModelMessage : ValueChangedMessage<List<ShiftsystemModel>>
{
    public ViewModelMessage(List<ShiftsystemModel> listOfShiftsystems) : base(listOfShiftsystems)
    {
        
    }
    
}