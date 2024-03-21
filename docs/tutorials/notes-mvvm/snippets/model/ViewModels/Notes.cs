using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class Notes
{
    public ObservableCollection<Note> AllNotes { get; } = new ObservableCollection<Note>();

    public ICommand NewCommand { get; }

    public ICommand SelectNoteCommand { get; }

    public Notes()
    {
        
    }
}
