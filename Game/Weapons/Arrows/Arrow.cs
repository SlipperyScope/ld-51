using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.Weapons
{
    /// <summary>
    /// Flying projectile typically fired from a bow
    /// </summary>
    public abstract class Arrow : KinematicBody2D
    {
        /// <summary>
        /// Entity responsible for the arrow
        /// </summary>
        [Export]
        private NodePath InstagatorPath = null;

        public Node Instagator { get; private set; } = null;

        /// <summary>
        /// Whether the arrow should be active when it becomes ready
        /// </summary>
        [Export]
        protected Boolean ActiveateOnReady { get; private set; } = false;

        /// <summary>
        /// Arrow velocity
        /// </summary>
        public Vector2 Velocity { get; set; } = Vector2.Zero;

        /// <summary>
        /// Whether the arrow is active
        /// </summary>
        public Boolean Active { get; private set; }

        public override void _Ready()
        {
            Instagator = GetNode(InstagatorPath);
        }

        /// <summary>
        /// Activates the arrow
        /// </summary>
        public virtual void Activate()
        {
            if (Active is false)
            {
                Active = true;
                CollisionLayer |= Core.Physics.Layers[Physics.Layer.ActiveArrow];
            }
        }

        /// <summary>
        /// Deactivatres the arrow
        /// </summary>
        public virtual void Deactivate()
        {
            if (Active is true)
            {
                Active = false;
                CollisionLayer &= ~Core.Physics.Layers[Physics.Layer.ActiveArrow];
            }
        }
    }
}
