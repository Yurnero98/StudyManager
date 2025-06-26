public interface IFileDialogService
{
    string? ShowSaveFileDialog(string filter);
    string? ShowOpenFileDialog(string filter);
}
