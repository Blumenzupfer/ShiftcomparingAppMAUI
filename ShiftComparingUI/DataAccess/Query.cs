namespace ShiftComparingUI.ShiftComparingLibrary.DataAccess;

public static class Query
{
    public const string CreateShiftsystemTable = @"CREATE TABLE IF NOT EXISTS Shiftsystem (
            Id INTEGER NOT NULL,
            Name TEXT NOT NULL,
            Description TEXT NOT NULL,
            Shiftpattern TEXT NOT NULL,
            ShiftgroupStartdates TEXT NOT NULL,
            PRIMARY KEY(Id AUTOINCREMENT))";
    
    public const string CreatePersonTable = @"CREATE TABLE IF NOT EXISTS Person (
            Id INTEGER NOT NULL,
            Firstname TEXT NOT NULL,
            Lastname TEXT NOT NULL,
            Nickname TEXT NOT NULL,
            Shiftsystem INTEGER,
            Shiftgroup INTEGER,
            FOREIGN KEY(Shiftsystem) REFERENCES Shiftsystem(Id),
            PRIMARY KEY(Id AUTOINCREMENT))";
}