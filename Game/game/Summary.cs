using Godot;
using System;

public class Summary : Control
{
	public string GameScene = "res://game/Game.tscn";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<RichTextLabel>("Score").BbcodeText = $"[center]{Global.score:F}[/center]";
        GetNode<RichTextLabel>("Arrows").BbcodeText = $"[center]{Global.arrowsShottened}[/center]";
        GetNode<RichTextLabel>("Apples").BbcodeText = $"[center]{Global.apples}[/center]";
		GetNode<Button>("Restart").Connect("pressed", this, nameof(Restart));
    }

    private void Restart() {
        Global.apples = 0;
        Global.arrowsShottened = 0;
        Global.score = 0;
        Global.level = 0;

        GetTree().ChangeScene(GameScene);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//
//  }
}
