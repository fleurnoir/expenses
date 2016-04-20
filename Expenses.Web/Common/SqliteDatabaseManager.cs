using System;
using Expenses.BL.Entities;
using System.IO;
using System.Data.SQLite;

namespace Expenses.Web
{
    public class SqliteDatabaseManager : IDatabaseManager
    {
        private string m_folder;

        private const string CreateTablesQuery = @"
        
CREATE TABLE `Users` (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `Login` TEXT NOT NULL UNIQUE,
    `PasswordHash`  TEXT NOT NULL,
    `FirstName` TEXT,
    `LastName`  TEXT,
    `Email` TEXT,
    `Comment`   TEXT
);

CREATE TABLE `Categories` (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `Name`  TEXT NOT NULL UNIQUE,
    `Type`  INTEGER NOT NULL,
    `Comment`   TEXT
);

CREATE TABLE `Subcategories` (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `Name`  TEXT NOT NULL,
    `CategoryId`    INTEGER NOT NULL,
    `Comment`   TEXT,
    FOREIGN KEY(`CategoryId`) REFERENCES Categories ( Id )
);

CREATE TABLE `Currencies` (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `ShortName` TEXT NOT NULL UNIQUE,
    `Name`  TEXT NOT NULL UNIQUE,
    `Comment`   TEXT
);

CREATE TABLE `Accounts` (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `Name`  TEXT NOT NULL UNIQUE,
    `CurrencyId`    INTEGER NOT NULL,
    `Amount`    REAL NOT NULL,
    `Comment`   TEXT,
    FOREIGN KEY(`CurrencyId`) REFERENCES Currencies ( Id )
);

CREATE TABLE `Exchanges` (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `SourceAccountId`   INTEGER NOT NULL,
    `DestAccountId` INTEGER NOT NULL,
    `SourceAmount`  REAL NOT NULL,
    `DestAmount`    REAL NOT NULL,
    `OperationTime` REAL NOT NULL,
    `UserId`    INTEGER NOT NULL,
    `Comment`   TEXT,
    FOREIGN KEY(`SourceAccountId`) REFERENCES Accounts ( Id ),
    FOREIGN KEY(`DestAccountId`) REFERENCES Accounts ( Id ),
    FOREIGN KEY(`UserId`) REFERENCES Users ( Id )
);

CREATE TABLE `Operations` (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `OperationTime` REAL NOT NULL,
    `UserId`    INTEGER NOT NULL,
    `AccountId` INTEGER NOT NULL,
    `Amount`    REAL NOT NULL,
    `SubcategoryId` INTEGER NOT NULL,
    `Comment`   TEXT,
    FOREIGN KEY(`UserId`) REFERENCES Users ( Id ),
    FOREIGN KEY(`AccountId`) REFERENCES Accounts ( Id ),
    FOREIGN KEY(`SubcategoryId`) REFERENCES Subcategories ( Id )
);

CREATE TABLE `KeyValuePairs` (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `Key`   TEXT NOT NULL UNIQUE,
    `Value` TEXT
);

CREATE TABLE `Debts` (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `AgentName` TEXT NOT NULL,
    `AccountId` INTEGER NOT NULL,
    `Comment`   TEXT,
    `Amount`    REAL NOT NULL,
    `RepayedAmount` REAL NOT NULL,
    `UserId`    INTEGER NOT NULL,
    `OperationTime` REAL NOT NULL,
    `Type`  INTEGER NOT NULL,
    `Repayed`   INTEGER NOT NULL,
    FOREIGN KEY(`AccountId`) REFERENCES Accounts ( Id ),
    FOREIGN KEY(`UserId`) REFERENCES Users ( Id )
);

CREATE TABLE `Repayments` (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `Comment`   TEXT,
    `UserId`    INTEGER NOT NULL,
    `OperationTime` REAL NOT NULL,
    `DebtId`    INTEGER NOT NULL,
    `Amount`    REAL NOT NULL,
    FOREIGN KEY(`UserId`) REFERENCES Users(Id),
    FOREIGN KEY(`DebtId`) REFERENCES Debts(Id)
);

";

        public SqliteDatabaseManager(string folder) {
            if (folder == null)
                throw new ArgumentNullException (nameof(folder));
            m_folder = folder;
        }

        private string GetDbPath(User dbUser) 
        {
            return Path.Combine (m_folder, $"{dbUser.Login}.sqlite");
        }

        public bool DatabaseExists (User dbUser)
        {
            return File.Exists (GetDbPath(dbUser));
        }

        public void CreateDatabase (User dbUser)
        {
            var dbPath = GetDbPath (dbUser);
            SQLiteConnection.CreateFile (dbPath);
            using (var connection = new SQLiteConnection (GetConnectionString(dbPath))) 
            using (var command = connection.CreateCommand()) {
                command.CommandText = CreateTablesQuery;
                command.ExecuteNonQuery ();
            }
        }

        private const string ConnectionStringTemplate = "data source={0};foreign keys=true";

        private static string GetConnectionString (string dbPath)
        {
            return String.Format (ConnectionStringTemplate, dbPath);
        }

        public string GetConnectionString (User dbUser)
        {
            return GetConnectionString(GetDbPath(dbUser));
        }
    }
}

