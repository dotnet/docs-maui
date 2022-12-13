//<namespaces>
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;
using System.Windows.Input;
//</namespaces>

namespace Notes.ViewModels;

//<class>
internal class Notes : ObservableRecipient, IRecipient<Messages.NoteSaved>, IRecipient<Messages.NoteDeleted> 
//</class>
{
    public ObservableCollection<ViewModels.Note> AllNotes { get; } = new ObservableCollection<ViewModels.Note>();
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    //<isactive>
    public Notes()
    {
        AllNotes = new ObservableCollection<ViewModels.Note>(Models.Note.LoadAll().Select(x => new Note(x)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<ViewModels.Note>(SelectNoteAsync);
        IsActive = true;
    }
    //</isactive>

    public async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    public async Task SelectNoteAsync(ViewModels.Note note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.Identifier}");
    }

    //<notesaved>
    public void Receive(Messages.NoteSaved message)
    {
        Note matchedInstance = AllNotes.Where((n) => n.Identifier == message.Note.Identifier).FirstOrDefault();

        if (matchedInstance != null)
            AllNotes.Remove(matchedInstance);

        AllNotes.Insert(0, message.Note);
    }
    //</notesaved>

    //<notedeleted>
    public void Receive(Messages.NoteDeleted message)
    {
        Note matchedInstance = AllNotes.Where((n) => n.Identifier == message.Note.Identifier).FirstOrDefault();

        if (matchedInstance != null)
            AllNotes.Remove(matchedInstance);
    }
    //</notedeleted>
}