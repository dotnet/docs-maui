using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Notes.ViewModels;

internal class Note : ObservableObject, IQueryAttributable
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

    public DateTime Date
    {
        get => _note.Date;
        private set
        {
            _note.Date = value;
            OnPropertyChanged();
        }
    }

    public string Identifier => _note.Filename;

    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }

    public Note()
    {
        _note = new Models.Note();
        SetupCommands();
    }

    public Note(Models.Note note)
    {
        _note = note;
        SetupCommands();
    }

    private void SetupCommands()
    {
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    //<save>
    public async Task Save()
    {
        _note.Date = DateTime.Now;
        _note.Save();

        WeakReferenceMessenger.Default.Send<Messages.NoteSaved>(new Messages.NoteSaved(this));

        await Shell.Current.GoToAsync("..");
    }
    //</save>

    //<delete>
    public async Task Delete()
    {
        _note.Delete();
        
        WeakReferenceMessenger.Default.Send<Messages.NoteDeleted>(new Messages.NoteDeleted(this));
        
        await Shell.Current.GoToAsync("..");
    }
    //<delete>

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _note = Models.Note.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    public void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Date));
    }
}