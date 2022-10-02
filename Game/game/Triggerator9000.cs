using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.game
{
    public class Triggerator9000 : Node, ITriggerable
    {
        [Export]
        private NodePath Target;

        public void Trigger()
        {
            if (Target is null)
            {
                GD.PushWarning("Target is null");
            }
            else if (GetNode<Prop>(Target) is Prop prop)
            {
                prop.Enable();
                //prop.ApplyCentralImpulse(Vector2.Down);
            }
        }
    }
}
