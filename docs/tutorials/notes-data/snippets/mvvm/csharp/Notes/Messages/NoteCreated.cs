namespace Notes.Messages;

internal class NoteCreated
{
    public ViewModels.NoteVM Note { get; }

    public NoteCreated(ViewModels.NoteVM note) =>
        Note = note;
}
