using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace ld51.game
{
	public class Prop : RigidBody2D
	{
		[Export]
		private Boolean StartEnabled = true;
		public Boolean Enabled = false;

		[Export]
		public Boolean EnableOnHit = false;

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


		public override void _EnterTree()
		{
			Enabled = !StartEnabled;
			if (StartEnabled is false)
				Disable();
			else
				Enable();

		}

		public override void _PhysicsProcess(Single delta)
		{
			if (Enabled is false)
			{
				LinearVelocity = Vector2.Zero;
				AngularVelocity = 0f;
			}
		}

        public void Enable()
		{
			if (Enabled is true) return;

			if (IsConnected("body_entered", this, nameof(BodyEntered)) is false)
				Connect("body_entered", this, nameof(BodyEntered));
			ContactsReported = 8;
			Enabled = true;
			Sleeping = false;
			GravityScale = 1f;
		}

		public void Disable()
		{
			if (Enabled is false) return;

			if (IsConnected("body_entered", this, nameof(BodyEntered)) is true)
				Disconnect("body_entered", this, nameof(BodyEntered));
			ContactsReported = 0;
			Enabled = false;
			Sleeping = true;
			GravityScale = 0f;
		}

		private void BodyEntered(PhysicsBody2D body)
		{
			if (body is IAudioTriggerable triggerable)
			{
				triggerable.TriggerAudio();
			}
		}
	}
}
