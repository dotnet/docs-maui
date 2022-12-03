using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class Notes
{
    //<properties>
    public ObservableCollection<ViewModels.Note> AllNotes { get; } = new ObservableCollection<ViewModels.Note>();
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }
    //</properties>

    //<ctor>
    public Notes()
    {
        AllNotes = new ObservableCollection<ViewModels.Note>(Models.Note.LoadAll().Select(x => new Note(x)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<ViewModels.Note>(SelectNoteAsync);
    }
    //</ctor>

    //<commands>
    public async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    public async Task SelectNoteAsync(ViewModels.Note note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.Identifier}");
    }
    //</commands>
}
