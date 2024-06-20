//<full>
using CommunityToolkit.Mvvm.Input;
using Notes.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class NotesViewModel : IQueryAttributable
{
    //<properties>
    public ObservableCollection<ViewModels.NoteViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }
    //</properties>

    //<ctor>
    public NotesViewModel()
    {
        AllNotes = new ObservableCollection<ViewModels.NoteViewModel>(Models.Note.LoadAll().Select(n => new NoteViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<ViewModels.NoteViewModel>(SelectNoteAsync);
    }
    //</ctor>

    //<commands>
    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    private async Task SelectNoteAsync(ViewModels.NoteViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.Identifier}");
    }
    //</commands>

    //<query>
    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            NoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note exists, delete it
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            //<insert>
            //<move>
            string noteId = query["saved"].ToString();
            NoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

            // If note is found, update it
            if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }
            //</move>
            // If note isn't found, it's new; add it.
            else
                AllNotes.Insert(0, new NoteViewModel(Models.Note.Load(noteId)));
            //</insert>
        }
    }
    //</query>
}
//</full>
