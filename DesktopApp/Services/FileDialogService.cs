using Microsoft.Win32;

public class FileDialogService : IFileDialogService
{
    public string? ShowSaveFileDialog(string filter)
    {
        var dialog = new SaveFileDialog
        {
            Filter = filter
        };
        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    public string? ShowOpenFileDialog(string filter)
    {
        var dialog = new OpenFileDialog
        {
            Filter = filter
        };
        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }
}
