using ld51.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using ld51.Utils;

namespace ld51.Willie
{
    public class BodyPart : RigidBody2D, IDamageable
    {
        /// <summary>
        /// Nofities when damage is taken
        /// </summary>
        public event DamageTakenHandler DamageTaken;

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
            if (DetachOnHit is true)
            {
                Detach();
            }

            CallDeferred("apply_impulse", info.DamagePosition - GlobalPosition, info.Impulse);
            DamageTaken?.Invoke(this, new(info));
        }

        /// <summary>
        /// Detach joints
        /// </summary>
        public void Detach()
        {
            if (DetachJoints is not null)
            {
                foreach (var path in DetachJoints)
                {
                    var joint = GetNodeOrNull<Joint2D>(path);
                    joint?.QueueFree();
                }
            }
        }
    }
}
