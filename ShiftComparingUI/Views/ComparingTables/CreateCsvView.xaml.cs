using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShiftComparingUI.ViewModels.ComparingTable;

namespace ShiftComparingUI.Views.ComparingTables;

public partial class CreateCsvView : ContentPage
{
    public CreateCsvView(CreateCsvViewModel ccvm)
    {
        InitializeComponent();
        BindingContext = ccvm;
    }
}