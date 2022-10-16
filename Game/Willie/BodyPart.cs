using ld51.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using ld51.Utils;
using ld51.Entities;

namespace ld51.Willie
{
    public class BodyPart : RigidBody2D, IDamageable, IPaintable
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

        #region IDamageable
        public void Damage(DamageInfo info)
        {
            if (DetachOnHit is true)
            {
                Detach();
            }

            CallDeferred("apply_impulse", info.DamagePosition - GlobalPosition, info.Impulse);
            DamageTaken?.Invoke(this, new(info));
        } 
        #endregion

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

        #region IPaintable
        public void PaintDecal(Sprite decal, Transform2D transform)
        {
            if (decal.GetParent() is not null)
            {
                decal.Warn($"Decal still parented to {decal.GetParent().Name}");
            }
            else
            {
                AddChild(decal);
                decal.GlobalTransform = transform;
            }
        }
        #endregion
    }
}
