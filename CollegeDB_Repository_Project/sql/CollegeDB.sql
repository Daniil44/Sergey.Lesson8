IF DB_ID(N'CollegeDB') IS NULL
    CREATE DATABASE CollegeDB;
GO

USE CollegeDB;
GO

IF OBJECT_ID(N'dbo.Groups', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Groups
    (
        GroupId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Groups PRIMARY KEY,
        Name NVARCHAR(50) NOT NULL,
        Course INT NOT NULL CONSTRAINT CK_Groups_Course CHECK (Course BETWEEN 1 AND 6)
    );
END;
GO

IF OBJECT_ID(N'dbo.Teachers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Teachers
    (
        TeacherId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Teachers PRIMARY KEY,
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Students
    (
        StudentId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Students PRIMARY KEY,
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        GroupId INT NULL,
        CONSTRAINT FK_Students_Groups FOREIGN KEY (GroupId) REFERENCES dbo.Groups(GroupId)
    );
END;
GO

IF OBJECT_ID(N'dbo.Subjects', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Subjects
    (
        SubjectId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Subjects PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.Grades', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Grades
    (
        GradeId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Grades PRIMARY KEY,
        StudentId INT NOT NULL,
        SubjectId INT NOT NULL,
        TeacherId INT NULL,
        GradeValue INT NOT NULL CONSTRAINT CK_Grades_Value CHECK (GradeValue BETWEEN 1 AND 5),
        GradeDate DATE NOT NULL CONSTRAINT DF_Grades_Date DEFAULT (CONVERT(date, GETDATE())),
        CONSTRAINT FK_Grades_Students FOREIGN KEY (StudentId) REFERENCES dbo.Students(StudentId),
        CONSTRAINT FK_Grades_Subjects FOREIGN KEY (SubjectId) REFERENCES dbo.Subjects(SubjectId),
        CONSTRAINT FK_Grades_Teachers FOREIGN KEY (TeacherId) REFERENCES dbo.Teachers(TeacherId)
    );
END;
GO

-- Необязательные тестовые данные:
IF NOT EXISTS (SELECT 1 FROM dbo.Groups WHERE Name = N'ИС-21')
    INSERT INTO dbo.Groups (Name, Course) VALUES (N'ИС-21', 2);
GO
