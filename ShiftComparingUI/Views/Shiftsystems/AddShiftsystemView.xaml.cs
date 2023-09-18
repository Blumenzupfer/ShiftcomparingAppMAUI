using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShiftComparingUI.ShiftComparingLibrary.ViewModels;

namespace ShiftComparingUI.Views.Shiftsystems;

public partial class AddShiftsystemView : ContentPage
{
    public AddShiftsystemView(AddShiftsystemViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}