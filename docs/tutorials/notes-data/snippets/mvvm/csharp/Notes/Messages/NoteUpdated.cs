namespace Notes.Messages;

internal class NoteUpdated
{
    public ViewModels.NoteVM Note { get; }

    public NoteUpdated(ViewModels.NoteVM note) =>
        Note = note;
}
