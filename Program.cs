using System;
using System.Windows.Forms;
using System.IO;

namespace CMS
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Set the data directory path
            AppDomain.CurrentDomain.SetData("DataDirectory", @"D:\CMS");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize database
            InitializeDatabase();

            Application.Run(new LoginForm());
        }

        static void InitializeDatabase()
        {
            try
            {
                // Ensure the directory exists
                string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
                if (!Directory.Exists(dataDirectory))
                {
                    Directory.CreateDirectory(dataDirectory);
                }

                // Create tables if they don't exist
                string createTables = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                CREATE TABLE Users (
                    UserID INT IDENTITY(1,1) PRIMARY KEY,
                    Username NVARCHAR(50) UNIQUE NOT NULL,
                    Password NVARCHAR(255) NOT NULL,
                    Email NVARCHAR(100) NOT NULL,
                    FirstName NVARCHAR(50) NOT NULL,
                    LastName NVARCHAR(50) NOT NULL,
                    CreatedDate DATETIME DEFAULT GETDATE()
                );

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Customers' AND xtype='U')
                CREATE TABLE Customers (
                    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    Email NVARCHAR(100) NULL,
                    Phone NVARCHAR(20) NULL,
                    Address NVARCHAR(255) NULL,
                    CreatedDate DATETIME DEFAULT GETDATE(),
                    CreatedBy INT NULL
                );

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
                CREATE TABLE Products (
                    ProductID INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    Price DECIMAL(18,2) NOT NULL,
                    Quantity INT NOT NULL,
                    Description NVARCHAR(500) NULL,
                    CreatedDate DATETIME DEFAULT GETDATE(),
                    CreatedBy INT NULL
                );

                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Orders' AND xtype='U')
                CREATE TABLE Orders (
                    OrderID INT IDENTITY(1,1) PRIMARY KEY,
                    CustomerID INT NOT NULL,
                    ProductID INT NOT NULL,
                    Quantity INT NOT NULL,
                    TotalAmount DECIMAL(18,2) NOT NULL,
                    OrderDate DATETIME DEFAULT GETDATE(),
                    CreatedBy INT NULL,
                    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
                    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
                );";

                // Check if admin user exists, if not create one
                string createAdmin = @"
                IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'admin')
                INSERT INTO Users (Username, Password, Email, FirstName, LastName) 
                VALUES ('admin', 'admin123', 'admin@cms.com', 'System', 'Administrator');";

                // Execute table creation
                DatabaseHelper.ExecuteNonQuery(createTables);

                // Create admin user
                DatabaseHelper.ExecuteNonQuery(createAdmin);

                MessageBox.Show("Database initialized successfully!", "Success",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database initialization error: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}