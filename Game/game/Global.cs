using Godot;
using System;

public class Global : Node
{
    public static Game game;

    public static int level = 0;
    public static float timeRemaining = 0;
    public static int arrowsShottened = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public static void Win() {
        if (game != null) {
            game.NextLevel();
        } else {
            throw new Exception("Tried to win a level before seting game on Global");
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//
//  }
}
