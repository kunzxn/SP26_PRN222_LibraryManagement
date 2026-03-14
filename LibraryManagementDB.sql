-- 1. Create Database
CREATE DATABASE LibraryManagementDB;
GO

USE LibraryManagementDB;
GO

-- 2. Create Users Table
CREATE TABLE Users (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL DEFAULT 'Student',
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- 3. Create Books Table
CREATE TABLE Books (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(150) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    ISBN NVARCHAR(20) NOT NULL UNIQUE,
    Quantity INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- 4. Create BorrowRecords Table
CREATE TABLE BorrowRecords (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    BookID INT NOT NULL,
    BorrowDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    DueDate DATETIME2 NOT NULL,
    Returned BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_BorrowRecords_Users FOREIGN KEY (UserID) REFERENCES Users(ID) ON DELETE NO ACTION,
    CONSTRAINT FK_BorrowRecords_Books FOREIGN KEY (BookID) REFERENCES Books(ID) ON DELETE NO ACTION
);
GO

-- 5. Create Sessions Table (For Authentication)
CREATE TABLE Sessions (
    SessionID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserID INT NOT NULL,
    Role NVARCHAR(20) NOT NULL,
    ExpiresAt DATETIME2 NOT NULL,
    CONSTRAINT FK_Sessions_Users FOREIGN KEY (UserID) REFERENCES Users(ID) ON DELETE CASCADE
);
GO

-- 6. Insert Default Admin Data
-- Password is 'admin123' hashed with BCrypt
INSERT INTO Users (FullName, Email, Password, Role, CreatedAt)
VALUES ('Admin', 'admin@university.edu', '$2a$11$91J9xW56JstAUKCokqDk8eOQx3/F2S3B4H7t7fG5hSInuMRE1234.', 'Admin', GETDATE());
GO

-- Insert More Sample Books
INSERT INTO Books (Title, Author, Category, ISBN, Quantity, CreatedAt)
VALUES
('The Art of Computer Programming', 'Donald Knuth', 'Algorithms', '978-0201896831', 1, GETDATE()),
('Algorithms', 'Robert Sedgewick', 'Algorithms', '978-0321573513', 4, GETDATE()),
('Grokking Algorithms', 'Aditya Bhargava', 'Algorithms', '978-1617292231', 6, GETDATE()),
('Computer Graphics', 'Donald Hearn', 'Graphics', '978-0133305968', 2, GETDATE()),
('Game Programming Patterns', 'Robert Nystrom', 'Game Development', '978-0990582908', 5, GETDATE()),
('Unity in Action', 'Joseph Hocking', 'Game Development', '978-1617294969', 3, GETDATE()),
('Learning React', 'Alex Banks', 'Web Development', '978-1492051725', 4, GETDATE()),
('JavaScript: The Good Parts', 'Douglas Crockford', 'Web Development', '978-0596517748', 5, GETDATE()),
('You Don''t Know JS', 'Kyle Simpson', 'Web Development', '978-1491904244', 4, GETDATE()),
('ASP.NET Core in Action', 'Andrew Lock', 'Web Development', '978-1617294617', 3, GETDATE()),
('Pro Blazor', 'Chris Sainty', 'Web Development', '978-1484278443', 2, GETDATE()),
('C# in Depth', 'Jon Skeet', 'Programming', '978-1617294532', 4, GETDATE()),
('Effective Java', 'Joshua Bloch', 'Programming', '978-0134685991', 3, GETDATE()),
('Programming Rust', 'Jim Blandy', 'Programming', '978-1492052586', 2, GETDATE()),
('Clean Architecture', 'Robert C. Martin', 'Software Architecture', '978-0134494166', 5, GETDATE());
