using Godot;
using System;

public class Menu : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	[Export]
	public string GameScene = "res://game/Game.tscn";

	[Export]
	public string InstructionsScene = "res://game/Instructions.tscn";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		string P = "VBoxContainer/";
		GetNode<Button>(P + "Play").Connect("pressed", this, nameof(GotoScene), new Godot.Collections.Array { GameScene });
		GetNode<Button>(P + "Instructions").Connect("pressed", this, nameof(GotoScene), new Godot.Collections.Array { InstructionsScene });
		GetNode<Button>(P + "Exit").Connect("pressed", this, nameof(Exit));
	}

	private void GotoScene(string Scene) {
		GetTree().ChangeScene(Scene);
	}

	private void Exit() {
		GetTree().Quit();
	}
}
