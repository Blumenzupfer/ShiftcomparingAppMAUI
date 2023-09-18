using Newtonsoft.Json;
using ShiftComparingUI.ShiftComparingLibrary.HelperClasses;

namespace ShiftComparingUI.ShiftComparingLibrary.Models;

public class ShiftsystemModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Shiftpattern { get; set; } // list needs to be a string due sqlite database
    public string ShiftgroupStartdates { get; set; } // list needs to be a string due sqlite database

    public static string ListToString(List<string> list)
    {
        return JsonConvert.SerializeObject(list);
    }

    public static List<string> StringToList(string list)
    {
        return JsonConvert.DeserializeObject<List<string>>(list);
    }

    public static string ShiftgroupStartdatesToString(List<ShiftgroupDatetimes> shiftgroupStartdates)
    {
        return JsonConvert.SerializeObject(shiftgroupStartdates);
    }

    public static List<ShiftgroupDatetimes> ShiftgroupStartdatesToString(string shiftgroupStartdates)
    {
        return JsonConvert.DeserializeObject<List<ShiftgroupDatetimes>>(shiftgroupStartdates);
    }
}

