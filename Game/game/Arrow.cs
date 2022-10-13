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

        public Boolean IgnoreWillie = false;

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

            var sprite = GetNode<Sprite>("Sprite");
            var transform = sprite.GlobalTransform;
            var disable = true;
            var free = true;

            switch (collision.Collider)
            {
                case Bouncer bouncer:
                    Velocity = Velocity.Bounce(collision.Normal);
                    Move(collision.Remainder);
                    bouncer.TriggerAudio();
                    disable = false;
                    free = false;
                    break;

                case Apple apple:
                    apple.AddDecal(sprite);
                    if (apple.EnableOnHit is true && Input.IsKeyPressed((Int32)KeyList.Q) is false)
                        apple.Enable();
                    if (apple.Enabled && Input.IsKeyPressed((Int32)KeyList.Q) is false)
                        apple.ApplyImpulse(collision.Position - apple.GlobalPosition, Velocity);
                    apple.Touch(new() { Location = collision.Position, Direction = -collision.Normal, Velocity = Velocity });
                    break;

                case Gib gib:
                    gib.AddDecal(sprite);
                    if (gib.EnableOnHit is true && Input.IsKeyPressed((Int32)KeyList.Q) is false)
                        gib.Enable();
                    if (gib.Enabled && Input.IsKeyPressed((Int32)KeyList.Q) is false)
                        gib.ApplyImpulse(collision.Position - gib.GlobalPosition, Velocity);
                    gib.Touch(new() { Location = collision.Position, Direction = -collision.Normal, Velocity = Velocity });
                    break;

                case Prop prop:
                    prop.AddDecal(sprite);
                    if (prop.EnableOnHit is true)
                        prop.Enable();
                    if (prop.Enabled)
                    {
                        prop.ApplyImpulse(collision.Position - prop.GlobalPosition, Velocity);
                        GD.Print($"{prop}{prop.Name}");
                    }
                    if (prop is IGoal goal)
                        goal.Touch(new() { Location = collision.Position, Direction = -collision.Normal, Velocity = Velocity});
                    break;

                case RigidBody2D rigid:
                    Enabled = false;
                    RemoveChild(sprite);
                    rigid.AddChild(sprite);
                    rigid.ApplyImpulse(collision.Position - rigid.GlobalPosition, Velocity);
                    sprite.GlobalTransform = transform;
                    break;

                default:
                    RemoveChild(sprite);
                    GetParent<Node>().AddChild(sprite);
                    sprite.GlobalTransform = transform;
                    break;
            }

            if (disable is true)
            {
                Enabled = false;
            }

            if (free is true)
            {
                QueueFree();
            }
        }
    }
}
