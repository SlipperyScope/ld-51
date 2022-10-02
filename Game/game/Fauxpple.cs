using Godot;
using System;

public class Fauxpple : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Input(InputEvent ev) {
        if (ev is InputEventMouseButton mev && mev.Pressed && (ButtonList)mev.ButtonIndex == ButtonList.Left) {
            if (GetNode<Sprite>("Apple").GetRect().HasPoint(ToLocal(mev.Position))) {
                Global.Win();
            }
        }
    }
}
