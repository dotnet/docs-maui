namespace Notes.Messages;

internal class NoteDeleted
{
    public ViewModels.NoteVM Note { get; }

    public NoteDeleted(ViewModels.NoteVM note) =>
        Note = note;
}
