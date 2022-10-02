using Godot;
using System;

public class HUD : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private RichTextLabel Level;
    private RichTextLabel Arrows;
    private RichTextLabel Time;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Level = GetNode<RichTextLabel>("Level");
        Arrows = GetNode<RichTextLabel>("Arrows");
        Time = GetNode<RichTextLabel>("Time");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Level.Text = $"{Global.level}";
        Arrows.Text = $"{Global.arrowsShottened}";
        Time.Text = $" {Global.timeRemaining:F}";
    }
}
