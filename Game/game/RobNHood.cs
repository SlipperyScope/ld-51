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

        private RigidBody2D Hood;
        private RigidBody2D Rob;

        public override void _Ready()
        {
            Hood = GetNode<RigidBody2D>("Hood");
            Rob = GetNode<RigidBody2D>("Rob");
        }

        public override void _Process(Single delta)
        {
            var center = Rob.GlobalPosition;
            var mouse = GetGlobalMousePosition();
            var dir = center.DirectionTo(mouse);

            if (center.DistanceTo(mouse) > MaxBowDistance)
            {
                Hood.GlobalPosition = center + dir * MaxBowDistance; 
            }
            else
            {
                Hood.GlobalPosition = mouse;
            }

            Hood.LookAt(Hood.GlobalPosition + dir);
        }

        public override void _Input(InputEvent e)
        {
            if (e.IsActionPressed("Shoot"))
            {
                SpawnArrow();
            }
        }

        private void SpawnArrow()
        {
            if (ArrowScene is null)
            {
                GD.PrintErr("Arrow scene is not specified");
                return;
            }

            var arrow = ArrowScene.Instance<Arrow>();
            var trans = Hood.GetNode<Position2D>("Spawn").GlobalTransform;
            GetTree().Root.AddChild(arrow);
            arrow.GlobalTransform = trans;
            arrow.Velocity = trans.x * 1400f;
        }
    }
}
