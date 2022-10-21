using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace Notes.ViewModels;

public class NoteVM : ObservableObject
{
    private Models.Note _note;

    public string Text
    {
        get => _note.Text;
        set
        {
            if (_note.Text != value)
            {
                _note.Text = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime Date => _note.Date;

    public string Identifier => _note.Filename;

    public bool IsNoteNew { get; }

    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }


    public NoteVM(Models.Note note)
    {
        _note = note;
        SetupCommands();
    }

    public NoteVM()
    {
        _note = new Models.Note()
        {
            Filename = $"{Path.GetRandomFileName()}.notes.txt",
            Date = DateTime.Now,
            Text = ""
        };
        IsNoteNew = true;

        SetupCommands();
    }

    private void SetupCommands()
    {
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete, () => !IsNoteNew);
    }

    public async Task Save()
    {
        _note.Save();
        if (IsNoteNew)
            WeakReferenceMessenger.Default.Send(new Messages.NoteCreated(this));
        else
            WeakReferenceMessenger.Default.Send(new Messages.NoteUpdated(this));

        await Shell.Current.GoToAsync("..");
    }

    public async Task Delete()
    {
        _note.Delete();
        WeakReferenceMessenger.Default.Send(new Messages.NoteDeleted(this));
        await Shell.Current.GoToAsync("..");
    }

    public void Refresh()
    {
        _note = Models.Note.Load(_note.Filename);
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Date));
    }
}
