using JashwanthMilestone3.Models;

namespace JashwanthMilestone3.Services;

public class AppDataStore
{
    public List<User> Users { get; } = [];
    public List<Note> Notes { get; } = [];

    private int _userId = 1;
    private int _noteId = 1;

    public int NextUserId() => _userId++;
    public int NextNoteId() => _noteId++;
}
