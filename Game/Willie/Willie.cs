using Godot;
using ld51.game;
using System;

/// <summary>
/// Player-possessible entity that fires arrows
/// </summary>
public class Willie : Area2D
{
    /// <summary>
    /// ENTER TREE
    /// </summary>
    public override void _EnterTree()
    {
        Connect("body_entered", this, nameof(OnBodyEntered));
        Connect("body_exited", this, nameof(OnBodyExited));
    }

    /// <summary>
    /// Physics body entered Willie's range
    /// </summary>
    /// <param name="body">Physics body</param>
    private void OnBodyEntered(PhysicsBody2D body)
    {
        if (body is Arrow arrow)
        {
            arrow.IgnoreWillie = true;
        }
    }

    /// <summary>
    /// Physics body exited Willie's range
    /// </summary>
    /// <param name="body">Physics body</param>
    private void OnBodyExited(PhysicsBody2D body)
    {
        if (body is Arrow arrow)
        {
            arrow.IgnoreWillie = false;
        }
    }

    public override void _Process(Single delta)
    {
        Update();
    }

    public override void _Draw()
    {
        var s1 = GetNode<DampedSpringJoint2D>("ChinSpring");
        var s2 = GetNode<DampedSpringJoint2D>("ChinSpring2");

        
    }
}
