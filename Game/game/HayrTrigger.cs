using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.game
{
    public class HayrTrigger : RigidBody2D
    {
        [Export]
        private List<NodePath> Triggerables = new();

        public override void _EnterTree()
        {
            GetNode<Area2D>("Trigger").Connect("body_entered", this, nameof(OnBodyEntered));
        }

        private void OnBodyEntered(PhysicsBody2D body)
        {
            if (body is Arrow arrow)
            {
                foreach (var path in Triggerables)
                {
                    if (GetNode(path) is ITriggerable triggerable)
                    triggerable.Trigger();
                }
            }
        }
    }
}
