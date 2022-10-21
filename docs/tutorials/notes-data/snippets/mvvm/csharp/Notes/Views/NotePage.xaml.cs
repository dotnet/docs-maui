namespace Notes.Views;

[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class NotePage : ContentPage
{
    public string ItemId
    {
        set
        {
            if (value == "new")
                // Create a new viewmodel instance which loads a blank model
                BindingContext = new ViewModels.NoteVM();

            else
                // Create a viewmodel instance from a record ID (file name)
                BindingContext = new ViewModels.NoteVM(Models.Note.Load(value));
        }
    }

    public NotePage()
    {
        InitializeComponent();
    }
}