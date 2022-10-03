using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.game
{
    public class RobNHood : Node2D
    {
        [Export]
        private PackedScene ArrowScene;

        [Export]
        private PackedScene GibScene;

        [Export]
        private Single MaxBowDistance = 500f;

        [Export]
        private Single MinAngle = -45f;

        [Export]
        private Single MaxAngle = 45f;

        [Export]
        private Single DrawTime = 0.5f;

        private Hood Hood;
        private Rob Rob;
        private AudioStreamPlayer SFX;
        private Boolean Dead = false;
        private UInt64 StartDraw = 0;

        public override void _Ready()
        {
            Hood = GetNode<Hood>("Hood");
            Rob = GetNode<Rob>("Rob");
            SFX = GetNode<AudioStreamPlayer>("SFX");
        }

        public override void _Process(Single delta)
        {
            var center = Rob.GlobalPosition;
            var mouse = GetGlobalMousePosition();
            var dir = center.DirectionTo(mouse);
            var angle = Vector2.Right.AngleTo(dir);
            var max = Mathf.Deg2Rad(MaxAngle);
            var min = Mathf.Deg2Rad(MinAngle);
            if (angle > max)
            {
                dir = Vector2.Right.Rotated(max);
            }
            else if (angle < min)
            {
                dir = Vector2.Right.Rotated(min);
            }

            Hood.GlobalPosition = center + dir * Mathf.Min(center.DistanceTo(mouse), MaxBowDistance);
            Hood.LookAt(Hood.GlobalPosition + dir);

            if (StartDraw != 0)
            {
                Bownimate();
            }
        }

        private void Bownimate()
        {
            var delta = Time.GetTicksMsec() - StartDraw;
            Hood.SetAnim(Mathf.Min(1f, delta / (DrawTime * 1000f)));
        }

        public override void _Input(InputEvent e)
        {
            if (Dead is true) return;

            var delta = Time.GetTicksMsec() - StartDraw;

            if (e.IsActionPressed("Shoot"))
            {
                StartDraw = Time.GetTicksMsec();
            }
            else if (e.IsActionReleased("Shoot"))
            {
                SpawnArrow(Mathf.Min(1f, delta / (DrawTime * 1000f)));

                if (delta / (DrawTime * 1000f) <= 0.1f)
                { 
                    Rob.Dangit();
                }
                StartDraw = 0;
                Hood.SetAnim(0f);
            }
        }

        private void SpawnArrow(Single power)
        {
            if (ArrowScene is null)
            {
                GD.PrintErr("Arrow scene is not specified");
                return;
            }

            if (Global.paused is true) return;

            var arrow = ArrowScene.Instance<Arrow>();
            var trans = Hood.SpawnPosition.GlobalTransform;
            GetParent().AddChild(arrow);
            arrow.GlobalTransform = trans;
            arrow.Velocity = trans.x * 2000f * power;
            SFX.Play();
            Global.arrowsShottened++;
        }

        public void Kill()
        {
            Dead = true;
            if (GibScene is null)
            {
                GD.PrintErr("Gibs scene is not specified");
                return;
            }

            var gibs = GibScene.Instance() as Node2D;
            GetParent().AddChild(gibs);
            gibs.GlobalTransform = GlobalTransform;

            var trans = Hood.GlobalTransform;
            RemoveChild(Hood);
            GetParent().AddChild(Hood);
            Hood.Mode = RigidBody2D.ModeEnum.Rigid;
            Hood.GlobalTransform = trans;
            Hood.CollisionLayer = 2;
            Hood.CollisionMask |= 4;
            QueueFree();
        }
    }
}
