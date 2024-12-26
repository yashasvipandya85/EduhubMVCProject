use Eduhubmvc;
CREATE TABLE Enrollments (
    EnrollmentId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    CourseId INT NOT NULL,
    EnrollmentDate DATETIME DEFAULT GETDATE(),
    Status VARCHAR(20) CHECK (Status IN ('Accepted', 'Rejected', 'Pending')) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId)
);

CREATE TABLE Materials (
    MaterialId INT PRIMARY KEY IDENTITY(1,1),
    CourseId INT NOT NULL,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    URL VARCHAR(255) NULL,
    UploadDate DATETIME DEFAULT GETDATE(),
    ContentType VARCHAR(50) NOT NULL,
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId)
);

CREATE TABLE Enquiries (
    EnquiryId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    CourseId INT NULL,
    Subject VARCHAR(255) NOT NULL,
    Message TEXT NOT NULL,
    EnquiryDate DATETIME DEFAULT GETDATE(),
    Status VARCHAR(50) CHECK (Status IN ('Open', 'Closed', 'In Progress')) NOT NULL,
    Response TEXT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId)
);

CREATE TABLE Feedback (
    FeedbackId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Feedback TEXT NOT NULL,
    Date DATE NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);