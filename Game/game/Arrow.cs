using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace ld51.game
{
    public class Arrow : KinematicBody2D
    {
        [Export]
        public Vector2 Velocity = Vector2.Zero;
        public Vector2 Gravity = Vector2.Down * 980f;
        private Int32 MoveAttempts = 5;
        private Int32 Attempt = 0;
        private Boolean Enabled = true;
        private Single SpawnTime;

        public override void _Ready()
        {
            SpawnTime = Time.GetTicksMsec();
        }

        public override void _PhysicsProcess(Single delta)
        {
            if (Enabled is false) return;
            Attempt = 0;
            Move(Velocity * delta);
            Velocity += Gravity * delta;
            LookAt(GlobalPosition + Velocity);
        }

        private void Move(Vector2 amount)
        {
            if (amount.LengthSquared() < 1f) return;
            if (Attempt++ >= MoveAttempts) return;

            var collision = MoveAndCollide(amount, false);
            if (collision is null) return;
            
            switch (collision.Collider)
            {
                case Bouncer bouncer:
                    Velocity = Velocity.Bounce(collision.Normal);
                    Move(collision.Remainder);
                    bouncer.TriggerAudio();
                    break;

                case Prop prop:
                    prop.AddDecal(GetNode<Sprite>("Sprite"));
                    if (prop.EnableOnHit is true)
                        prop.Enable();
                    if (prop.Enabled)
                        prop.ApplyImpulse(collision.Position - prop.GlobalPosition, Velocity);
                    if (prop is IGoal goal)
                    {
                        goal.Touch(collision.Position);
                    }
                    QueueFree();
                    break;

                default:
                    Enabled = false;
                    var sprite2 = GetNode<Sprite>("Sprite");
                    var trans2 = sprite2.GlobalTransform;
                    RemoveChild(sprite2);
                    GetParent<Node>().AddChild(sprite2);
                    sprite2.GlobalTransform = trans2;
                    QueueFree();
                    break;
            }
        }
    }
}
