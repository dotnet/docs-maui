using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Notes.ViewModels;

internal class NotesVM : ObservableRecipient, IRecipient<Messages.NoteCreated>, IRecipient<Messages.NoteDeleted>, IRecipient<Messages.NoteUpdated>
{
    public ObservableCollection<NoteVM> Notes { get; } = new ObservableCollection<NoteVM>();

    public ICommand NewCommand { get; }

    public ICommand SelectNoteCommand { get; }

    public NotesVM()
    {
        Notes = new ObservableCollection<NoteVM>(Models.Note.LoadAll().Select(x => new NoteVM(x)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<ViewModels.NoteVM>(SelectNoteAsync);
        IsActive = true;
    }

    public async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?{nameof(Views.NotePage.ItemId)}=new");
    }

    public async Task SelectNoteAsync(ViewModels.NoteVM note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?{nameof(Views.NotePage.ItemId)}={note.Identifier}");
    }

    public void Receive(Messages.NoteCreated message)
    {
        Notes.Add(message.Note);
    }

    public void Receive(Messages.NoteDeleted message)
    {
        NoteVM matchedInstance = null;

        foreach (var note in Notes)
        {
            if (note.Identifier == message.Note.Identifier)
            {
                matchedInstance = note;
                break;
            }
        }

        if (matchedInstance != null)
            Notes.Remove(matchedInstance);
    }
    public void Receive(Messages.NoteUpdated message)
    {
        foreach (var note in Notes)
        {
            if (note.Identifier == message.Note.Identifier)
            {
                note.Refresh();
                break;
            }
        }
    }
}
