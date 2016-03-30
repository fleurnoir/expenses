CREATE TABLE "Users" (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `Login` TEXT NOT NULL UNIQUE,
    `PasswordHash`  TEXT NOT NULL,
    `FirstName` TEXT,
    `LastName`  TEXT,
    `Email` TEXT,
    `Comment`   TEXT
);

CREATE TABLE "Categories" (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `Name`  TEXT NOT NULL UNIQUE,
    `Type`  INTEGER NOT NULL,
    `Comment`   TEXT
);

CREATE TABLE "Subcategories" (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `Name`  TEXT NOT NULL,
    `CategoryId`    INTEGER NOT NULL,
    `Comment`   TEXT,
    FOREIGN KEY(`CategoryId`) REFERENCES Categories ( Id )
);

CREATE TABLE "Currencies" (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `ShortName` TEXT NOT NULL UNIQUE,
    `Name`  TEXT NOT NULL UNIQUE,
    `Comment`   TEXT
);

CREATE TABLE "Accounts" (
    `Id`    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
    `Name`  TEXT NOT NULL UNIQUE,
    `CurrencyId`    INTEGER NOT NULL,
    `Amount`    REAL NOT NULL,
    `Comment`   TEXT,
    FOREIGN KEY(`CurrencyId`) REFERENCES Currencies ( Id )
);

CREATE TABLE "Exchanges" (
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

CREATE TABLE "Operations" (
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
