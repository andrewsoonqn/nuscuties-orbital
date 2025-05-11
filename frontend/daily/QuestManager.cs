using Godot;
using System;
using System.Collections.Generic;

public partial class QuestManager : Node
{
    [Signal]
    public delegate void ManagerQuestAddedEventHandler(int id);

    [Signal]
    public delegate void ManagerQuestRemovedEventHandler(int id);

    [Signal]
    public delegate void ManagerQuestEditedEventHandler(int id);

    // Maps id to Quest object
    private Dictionary<int, Quest> _quests = new Dictionary<int, Quest>();
    public static readonly int MaxQuests = 5;

    public override void _Ready()
    {
        this._quests = new Dictionary<int, Quest>(MaxQuests);
    }

    public void Submit(string title, string description)
    {
        Quest q =  new Quest(title, description);
        this._quests.Add(q.Id, q);
        EmitSignal("ManagerQuestAdded", q.Id);
    }

    public void Remove(int id)
    {
        this._quests.Remove(id);
        EmitSignal(nameof(ManagerQuestRemovedEventHandler), id);
    }

    public void Edit(int id, string newTitle, string newDescription)
    {
        Quest q = _quests[id];
        q.Title = newTitle;
        q.Description = newDescription;
        EmitSignal(nameof(ManagerQuestEditedEventHandler), id);
    }

    public void ToggleCompletion(int id)
    {
        Quest q = _quests[id];
        q.Completed = !q.Completed;
        EmitSignal(nameof(ManagerQuestEditedEventHandler), id);
    }

    public Quest Get(int id)
    {
        return _quests[id];
    }

    public Dictionary<int, Quest> GetQuests()
    { // TODO remove
        return _quests;
    }
}
