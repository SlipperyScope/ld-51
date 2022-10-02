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
        private Single MaxBowDistance = 500f;

        [Export]
        private Single MinAngle = -45f;

        [Export]
        private Single MaxAngle = 45f;

        private Hood Hood;
        private Rob Rob;
        private AudioStreamPlayer SFX;
        private Timer Timer;
        private Int32 Tick = 0;
        private Int32 MaxTick = 100;

        public override void _Ready()
        {
            Hood = GetNode<Hood>("Hood");
            Rob = GetNode<Rob>("Rob");
            SFX = GetNode<AudioStreamPlayer>("SFX");
            Timer = GetNode<Timer>("Timer");
            Timer.Connect("timeout", this, nameof(BowTick));
            Timer.WaitTime = 1f / MaxTick;
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
        }

        private void BowTick()
        {
            if (Tick++ < MaxTick)
            {
                Timer.Start();
                Hood.SetAnim((Single)Tick / MaxTick);
            }
        }

        public override void _Input(InputEvent e)
        {
            if (e.IsActionPressed("Shoot"))
            {
                Timer.Start();
            }
            else if (e.IsActionReleased("Shoot"))
            {
                Timer.Stop();
                SpawnArrow(Tick);

                if (((Single)Tick / MaxTick) <= 0.1f)
                { 
                    Rob.Dangit();
                }
                Tick = 0;
                Hood.SetAnim(0f);
            }
        }

        private void SpawnArrow(Int32 power = 1)
        {
            if (ArrowScene is null)
            {
                GD.PrintErr("Arrow scene is not specified");
                return;
            }

            var arrow = ArrowScene.Instance<Arrow>();
            var trans = Hood.SpawnPosition.GlobalTransform;
            GetTree().Root.AddChild(arrow);
            arrow.GlobalTransform = trans;
            arrow.Velocity = trans.x * 2000f / MaxTick * power;
            SFX.Play();
        }
    }
}
