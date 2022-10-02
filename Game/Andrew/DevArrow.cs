using Godot;
using System;

//TODO: change to kinematic because godot physics sucks
public class DevArrow : RigidBody2D
{
	public override void _EnterTree()
	{
		Connect("body_entered", this, nameof(OnBodyEntered));
	}

	public override void _Ready()
	{
		ApplyCentralImpulse(Transform.x * 4000f);
	}

	public override void _PhysicsProcess(Single delta)
	{
		if (Mode is ModeEnum.Rigid && LinearVelocity.Length() >= 500f)
			LookAt(GlobalPosition + LinearVelocity);
	}

	public override void _IntegrateForces(Physics2DDirectBodyState state)
	{
		AppliedTorque = 0f;
		AppliedForce = Vector2.Zero;

		base._IntegrateForces(state);
	}

	private void OnBodyEntered(PhysicsBody2D body)
	{
		if (LinearVelocity.Length() < 500f)
		{
			CallDeferred(nameof(DisablePhysics));
			// wiggle or something? 
		}

		switch (body)
		{
			case DevApple apple:
				//CallDeferred(nameof(DisablePhysics));
				apple.AddDecal(GetNode<Sprite>("Sprite"));
				//apple.ApplyImpulse(GlobalPosition - apple.GlobalPosition, LinearVelocity * Mass / apple.Mass);
				//apple.Bonk(GlobalPosition, LinearVelocity * Mass);
				//apple.CallDeferred(nameof(apple.Bonk), GlobalPosition, LinearVelocity * Mass);
				apple.Bonk();
				QueueFree();
				break;

			default:
				break;
		}
	}

	private void DisablePhysics()
	{
		Mode = ModeEnum.Static;
		CollisionLayer = 0;
		CollisionMask = 0;
		ContactMonitor = false;
		CustomIntegrator = true;
	}
}
