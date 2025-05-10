using Godot;
using System;
using System.Collections.Generic;

public partial class QuestManager : Node
{ 
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
    }

    public void Remove(int id)
    {
        this._quests.Remove(id);
    }

    public void Edit(int id, string newTitle, string newDescription)
    { 
        Quest q = _quests[id];
        q.Title = newTitle;
        q.Description = newDescription;
    }

    public void ToggleCompletion(int id)
    {
        Quest q = _quests[id];
        q.Completed = !q.Completed;
    }
}
