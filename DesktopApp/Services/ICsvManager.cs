using ModernDesign.DataBase;

public interface ICsvManager
{
    void ExportStudentsToCsv(IEnumerable<Student> students, string filePath);
    IEnumerable<Student> ImportStudentsFromCsv(string filePath, int groupId);
}
