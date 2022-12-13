namespace Notes.Messages;

internal class NoteSaved
{
    public ViewModels.Note Note { get; }

    public NoteSaved(ViewModels.Note note) =>
        Note = note;
}
