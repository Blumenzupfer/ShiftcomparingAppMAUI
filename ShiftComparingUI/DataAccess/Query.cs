namespace ShiftComparingUI.DataAccess;

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
            Name TEXT NOT NULL,
            TableName TEXT NOT NULL,
            ShiftsystemId INTEGER,
            Shiftgroup INTEGER,
            FOREIGN KEY(ShiftsystemId) REFERENCES Shiftsystem(Id),
            PRIMARY KEY(Id AUTOINCREMENT))";
}