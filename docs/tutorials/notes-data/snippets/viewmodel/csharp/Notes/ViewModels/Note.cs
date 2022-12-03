//<full>
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class Note : ObservableObject, IQueryAttributable
{
    private Models.Note _note;

    //<properties>
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
    //</properties>

    //<commands>
    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }
    //</commands>

    //<ctor>
    public Note(Models.Note note)
    {
        _note = note;
        SetupCommands();
    }

    public Note()
    {
        _note = new Models.Note();
        SetupCommands();
    }
    //</ctor>

    //<methods>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _note = Models.Note.Load(query["load"].ToString());
            RefreshProperties();
        }
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Date));
    }
    //</methods>

    //<command_methods>
    private void SetupCommands()
    {
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public async Task Save()
    {
        _note.Save();
        await Shell.Current.GoToAsync("..");
    }

    public async Task Delete()
    {
        _note.Delete();
        await Shell.Current.GoToAsync("..");
    }
    //</command_methods>
}
//</full>
