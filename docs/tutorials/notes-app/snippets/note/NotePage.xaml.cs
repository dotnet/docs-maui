//<full>
namespace Notes;

//<filename_variable>
public partial class NotePage : ContentPage
{
    string _fileName = Path.Combine(FileSystem.AppDataDirectory, "notes.txt");
//</filename_variable>

    //<load_note>
    public NotePage()
    {
        InitializeComponent();

        if (File.Exists(_fileName))
            TextEditor.Text = File.ReadAllText(_fileName);
    }
    //</load_note>

    //<buttons>
    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        // Save the file.
        File.WriteAllText(_fileName, TextEditor.Text);
    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        // Delete the file.
        if (File.Exists(_fileName))
            File.Delete(_fileName);

        TextEditor.Text = string.Empty;
    }
    //</buttons>
}
//</full>
