using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.game
{
    public class Trap : Node2D, ITriggerable
    {
        private PinJoint2D Latch;
        private Prop Door;

        public override void _Ready()
        {
            Latch = GetNode<PinJoint2D>("Latch");
            Door = GetNode<Prop>("Door");
        }

        public void Trigger()
        {
            //Latch.QueueFree();
            Latch.NodeB = Latch.NodeA;
            Door.Enable();
        }
    }
}
