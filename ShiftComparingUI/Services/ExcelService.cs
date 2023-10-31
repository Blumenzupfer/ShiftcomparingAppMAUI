using ClosedXML.Excel;
using ShiftComparingUI.DataAccess;
using ShiftComparingUI.Models;

namespace ShiftComparingUI.Services;

public class ExcelService
{
    private ComparingTableModel ComparingTable { get; set; }
    private string WorksheetName { get; set; }
    private string FolderPath { get; set; }
    private List<string> MonthNames { get; set; } = new();
    private IXLWorksheet _worksheet;
    private int _monthStartColumn = 1;
    private const int FirstRow = 1;
    private readonly int _displacementFactor;
    private readonly List<PersonModel> _personsToBeCompared;
    private readonly List<int> _personsShiftpatternIndexes = new();
    private readonly List<List<string>> _listOfShiftpattern = new();

    public ExcelService(ComparingTableModel comparingTable, string worksheetName, string folderPath)
    {
        ComparingTable = comparingTable;
        WorksheetName = worksheetName;
        FolderPath = folderPath;
        _personsToBeCompared = ComparingTable.PersonsToBeCompared;
        _displacementFactor = comparingTable.PersonsToBeCompared.Count + 1;
        for (var i = 1; i <= 12; i++)
        {
            if (MonthNames.Count == 0) MonthNames.Add("filler");
            MonthNames.Add(new DateTime(ComparingTable.Year, i, 1).ToString("MMMM"));
        }
    }

    public void CreateComparingTableInCsvFile()
    {
        Init();
        using (var workbook = new XLWorkbook())
        {
            CreateWorksheet(workbook);
            AddContentToWorksheet();
            SaveWorkbook(workbook);
        }
        Console.WriteLine("Excel file created");
    }
    
    private void Init()
    {
        foreach (var person in _personsToBeCompared)
        {
            var shiftsystem = ShiftsystemDataAccess.GetOneShiftsystem(person.ShiftsystemId);
            var startDates = ShiftsystemModel.ShiftgroupStartdatesToList(shiftsystem.ShiftgroupStartdates);
            var shiftpattern = ShiftsystemModel.StringToList(shiftsystem.Shiftpattern);
            var shiftpatternStartDate = startDates[person.Shiftgroup].Startdate;
            var comparingTableYear = new DateTime(ComparingTable.Year, 1, 1);
            var timeSpan = System.Math.Abs((int)(shiftpatternStartDate - comparingTableYear).TotalDays);
            var personIndex = _personsToBeCompared.IndexOf(person);
            _personsShiftpatternIndexes.Add(0);
            _listOfShiftpattern.Add(shiftpattern);
            
            if (shiftpatternStartDate < comparingTableYear)
            {
                _personsShiftpatternIndexes[personIndex] = timeSpan % shiftpattern.Count;
            } 
            else if (shiftpatternStartDate > comparingTableYear)
            {
                _personsShiftpatternIndexes[personIndex] = shiftpattern.Count - (timeSpan % shiftpattern.Count);
                if (shiftpattern.Count == _personsShiftpatternIndexes[personIndex])
                    _personsShiftpatternIndexes[personIndex] = 0;
            }
        }
    }

    private void CreateWorksheet(XLWorkbook workbook)
    {
        _worksheet = workbook.Worksheets.Add(WorksheetName);
    }

    private void AddContentToWorksheet()
    {
        for (var month = 1; month <= 12; month++)
        {
            int firstColumn = _monthStartColumn;
            int lastColumn = _monthStartColumn + _displacementFactor;
            var daysInMonth = DateTime.DaysInMonth(ComparingTable.Year, month);
            
            AddMonthNameHeader(firstColumn, lastColumn, month);
            AddPersonColumnHeader(firstColumn + 2);
            
            for (var day = 1; day <= daysInMonth; day++)
            {
                AddDayOfMonthColumn(firstColumn, day);
                AddWeekdayColumns(firstColumn + 1, month, day);
                AddPersonColumns(firstColumn + 2, day);
            }
            AdjustColumnWidth(firstColumn, lastColumn);
            _monthStartColumn = lastColumn + 1;
        }
    }

    private void AddMonthNameHeader(int firstColumn, int lastColumn, int month)
    {
        _worksheet.Range(FirstRow, firstColumn, FirstRow, lastColumn).Merge();
        _worksheet.Cell(1, firstColumn).Value = MonthNames[month];
    }
    
    private void AddPersonColumnHeader(int currentColumn)
    {
        foreach (var person in _personsToBeCompared)
        {
            _worksheet.Cell(FirstRow + 1, currentColumn + _personsToBeCompared.IndexOf(person)).Value = person.TableName;
        }
    }
    
    private void AddDayOfMonthColumn(int firstColumn, int day)
    {
        _worksheet.Cell(FirstRow + 1 + day, firstColumn).Value = day;
        _worksheet.Cell(FirstRow + 1 + day, firstColumn).Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;
    }

    private void AddWeekdayColumns(int secondColumn, int month, int day)
    {
        var dayOfWeek = new DateTime(ComparingTable.Year, month, day).DayOfWeek;
        _worksheet.Cell(FirstRow + 1 + day, secondColumn).Value = dayOfWeek.ToString().Substring(0, 3);
    }

    private void AddPersonColumns(int thirdColumn, int day)
    {
        for (var personIndex = 0; personIndex < ComparingTable.PersonsToBeCompared.Count; personIndex++)
        {
            var currentColumn = thirdColumn + personIndex;
            // var currentPerson = ComparingTable.PersonsToBeCompared[personIndex];
            // var shiftsystem = ShiftsystemDataAccess.GetOneShiftsystem(currentPerson.ShiftsystemId);
            var shiftpattern = _listOfShiftpattern[personIndex];
            _worksheet.Cell(FirstRow + 1 + day, currentColumn).Value = shiftpattern[_personsShiftpatternIndexes[personIndex]];
            _personsShiftpatternIndexes[personIndex]++;
            if (_personsShiftpatternIndexes[personIndex] == shiftpattern.Count) 
                _personsShiftpatternIndexes[personIndex] = 0;
        }
    }

    private void AdjustColumnWidth(int firstColumn, int lastColumn)
    {
        _worksheet.Columns(firstColumn, lastColumn).AdjustToContents();
    }
    
    private void SaveWorkbook(XLWorkbook workbook)
    {
        workbook.SaveAs(Path.Combine(FolderPath, $"{WorksheetName}.xlsx"));
    }
    
    
    

}