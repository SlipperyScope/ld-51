using Godot;
using System;

public class Summary : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<RichTextLabel>("Score").BbcodeText = $"[center]{Global.score:F}[/center]";
        GetNode<RichTextLabel>("Arrows").BbcodeText = $"[center]{Global.arrowsShottened}[/center]";
        GetNode<RichTextLabel>("Apples").BbcodeText = $"[center]{Global.apples}[/center]";
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//
//  }
}
