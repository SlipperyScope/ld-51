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
        private Boolean Enabled = true;

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
            Enabled = true;
            GravityScale = 1f;
        }

        public void Disable()
        {
            Enabled = false;
            GravityScale = 0f;
        }
    }
}
