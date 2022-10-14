using ld51.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace ld51.Willie
{
    public class BodyPart : RigidBody2D, IDamageable
    {
        /// <summary>
        /// Detach joints on hit
        /// </summary>
        [Export]
        private Boolean DetachOnHit = false;

        /// <summary>
        /// Joints to detach
        /// </summary>
        [Export]
        private List<NodePath> DetachJoints = new();

        public void Damage(DamageInfo info)
        {
            Detach();
            CallDeferred(nameof(ApplyImpulse), info.DamagePosition - GlobalPosition, info.Impulse);
        }

        /// <summary>
        /// Detach joints
        /// </summary>
        public void Detach()
        {
            if (DetachOnHit is true)
            {
                foreach (var path in DetachJoints)
                {
                    var joint = GetNodeOrNull<Joint2D>(path) ?? throw new ArgumentNullException($"{path} is not a Joint2D");
                    joint.NodeA = null;
                    joint.NodeB = null;
                    joint.QueueFree();
                }
            }
        }
    }
}
