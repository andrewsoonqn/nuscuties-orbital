using System;
using System.Text.Json.Serialization;

public class Quest
{
    private static int _id = 0;

    public int Id { get; set; } // TODO: Hide this
    public string Title {get; set;}
    public string Description {get; set;}
    public bool Completed {get; set;}

    public Quest(string title, string description)
    {
        this.Id = _id;
        _id++;
        this.Title = title;
        this.Description = description;
        this.Completed = false;
    }

    public Quest() { }
    
    public override bool Equals(object obj)
    {
        if (obj == null) 
        {
            return false;  
        }
        if (!(obj is Quest))
        {
            return false;
        }
        Quest other = (Quest)obj;
        return this.Id == other.Id;
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }

    public void ToggleCompletion()
    {
        this.Completed = !this.Completed;
    }
}