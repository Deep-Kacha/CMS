-- Users Table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- Customers Table
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Address NVARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedBy INT FOREIGN KEY REFERENCES Users(UserID)
);

-- Products Table
CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Quantity INT NOT NULL,
    Description NVARCHAR(500),
    CreatedDate DATETIME DEFAULT GETDATE(),
    CreatedBy INT FOREIGN KEY REFERENCES Users(UserID)
);

-- Insert sample data
INSERT INTO Users (Username, Password, Email, FirstName, LastName) 
VALUES ('admin', 'admin123', 'admin@cms.com', 'System', 'Administrator');

INSERT INTO Customers (Name, Email, Phone, Address, CreatedBy)
VALUES 
    ('John Doe', 'john@email.com', '123-456-7890', '123 Main St', 1),
    ('Jane Smith', 'jane@email.com', '123-456-7891', '456 Oak Ave', 1);

INSERT INTO Products (Name, Price, Quantity, Description, CreatedBy)
VALUES 
    ('Laptop', 999.99, 10, 'High-performance laptop', 1),
    ('Mouse', 29.99, 50, 'Wireless mouse', 1);