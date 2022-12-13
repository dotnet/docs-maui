namespace Notes.Messages;

internal class NoteDeleted
{
    public ViewModels.Note Note { get; }

    public NoteDeleted(ViewModels.Note note) =>
        Note = note;
}
