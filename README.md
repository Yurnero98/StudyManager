# StudyManager

Desktop application for managing students, groups, teachers and courses. Built with WPF using the MVVM design pattern and Entity Framework Core.

## Technologies

- WPF (.NET)
- MVVM Pattern
- Entity Framework Core
- MS SQL Server
- LocalDb

## Features

- Add, edit, and delete students
- Manage groups and assign students to groups
- Manage courses and assign them to groups
- Real-time data updates across views
- Data import/export via CSV
- Input validation and user-friendly error messages
- Separation of concerns via clean architecture
- Logging using Microsoft.Extensions.Logging

## 🔧 Getting Started

### Prerequisites

- .NET SDK 8.0

- MS SQL Server or LocalDB (default setup uses LocalDB)

### Clone the Repository

- git clone https://github.com/yourusername/StudyManager.git
- cd StudyManager

### Apply EF Core migrations (optional if database is empty):

- dotnet ef database update --project StudyManager

- Press F5 or click Start to run the application

### Configure the Database (optional)

By default, the app uses SQL Server LocalDB. If needed, update the connection string in appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=StudyManagerDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}

### 📂 CSV Import/Export

You can import/export student data via CSV files using the built-in file dialog. Ensure your CSV format matches the expected headers:

Example CSV format:
<pre> <code>``` FirstName,LastName John,Doe Jane,Smith ```</code> </pre>

## License

MIT
