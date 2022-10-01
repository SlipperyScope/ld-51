using Godot;
using System;

public class DevArrow : RigidBody2D
{
    public override void _Ready()
    {
        ApplyCentralImpulse(Transform.x * 5000f);
    }

    public override void _PhysicsProcess(Single delta)
    {
        LookAt(GlobalPosition + LinearVelocity);
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        AppliedTorque = 0f;
        AppliedForce = Vector2.Zero;


        base._IntegrateForces(state);
    }
}
