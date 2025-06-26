using System.IO;
using ModernDesign.DataBase;

public class CsvManager : ICsvManager
{
    public void ExportStudentsToCsv(IEnumerable<Student> students, string filePath)
    {
        var lines = students.Select(s => $"{s.FirstName},{s.LastName}");
        File.WriteAllLines(filePath, lines);
    }

    public IEnumerable<Student> ImportStudentsFromCsv(string filePath, int groupId)
    {
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length >= 2)
            {
                yield return new Student
                {
                    FirstName = parts[0],
                    LastName = parts[1],
                    GroupId = groupId
                };
            }
        }
    }
}
