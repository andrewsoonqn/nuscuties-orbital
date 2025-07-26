using Godot;

public partial class AudioManager : Node
{
    [Export] public AudioStreamPlayer SfxPlayer;
    [Export] public AudioStream ButtonClickSound;

    public override void _Ready()
    {
        GetTree().Root.ChildEnteredTree += OnChildEnteredTree;
        ConnectAllCurrentButtons();
    }

    public float GetVolume()
    {
        return ConvertDbToPercentage(SfxPlayer.VolumeDb);
    }
    public void SetVolume(float percentage)
    {
        SfxPlayer.VolumeDb = ConvertPercentageToDb(percentage);
    }

    private float ConvertPercentageToDb(float percentage)
    {
        if (percentage <= 0) return -80.0f;
        return Mathf.Log(percentage / 100.0f) * 20.0f;
    }

    private float ConvertDbToPercentage(float db)
    {
        if (db <= -80.0f) return 0.0f;
        return Mathf.Pow(10.0f, db / 20.0f) * 100.0f;
    }
    private void OnChildEnteredTree(Node node)
    {
        ConnectButtonsInNode(node);
    }

    public void ConnectButtonsInNode(Node node)
    {
        if (node.IsInGroup("sfx_buttons") && node is Button button)
        {
            button.Pressed += PlayButtonClick;
        }
        foreach (Node child in node.GetChildren())
        {
            ConnectButtonsInNode(child);
        }
    }

    private void ConnectAllCurrentButtons()
    {
        var buttons = GetTree().GetNodesInGroup("sfx_buttons");
        foreach (var node in buttons)
        {
            if (node is Button button)
                button.Pressed += PlayButtonClick;
        }
    }

    public void PlaySfx(AudioStream stream)
    {
        SfxPlayer.Stream = stream;
        SfxPlayer.Play();
    }

    public void PlayButtonClick()
    {
        PlaySfx(ButtonClickSound);
    }

    public void ConnectButtonToClickSound(Button button)
    {
        button.AddToGroup("sfx_buttons");
        button.Pressed += () => PlayButtonClick();
    }
}
