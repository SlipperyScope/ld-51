using Godot;
using System;

public class DevApple : RigidBody2D
{
    public void AddDecal(Sprite decal)
    {
        var transform = decal.GlobalTransform;
        if (decal.GetParent() is not null)
        {
            decal.GetParent().RemoveChild(decal);
        }

        AddChild(decal);
        decal.GlobalTransform = transform;
    }

    // public void Bonk(Vector2 globalPosition, Vector2 force)
    public void Bonk()
    {
        //Mode = ModeEnum.Rigid;
        //CallDeferred("apply_impulse", globalPosition - GlobalPosition, force);
        //ApplyImpulse(GlobalPosition - globalPosition, force);
        GravityScale = 1f;
    }
}
