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
        /// Path to butt position node
        /// </summary>
        protected NodePath ButtPositionPath = new("Butt");

        /// <summary>
        /// Butt position node
        /// </summary>
        protected Position2D ButtOffsetNode;

        /// <summary>
        /// Offset from arrow origin of butt
        /// </summary>
        public Vector2 ButtOffset => ButtOffsetNode.Position;

        /// <summary>
        /// Path to instigator
        /// </summary>
        [Export]
        private NodePath InstigatorPath;

        /// <summary>
        /// Entity responsible for damage
        /// </summary>
        public Node Instigator { get; set; } = null;

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

        /// <summary>
        /// True if the arrow is held (no gravity)
        /// </summary>
        [Export]
        public Boolean Held { get; set; } = true;

        private Vector2 GravityVector;
        private Single GravityStrength;

        public Arrow()
        {
            Deactivate();
        }

        public override void _Ready()
        {
            GravityStrength = (Single)Physics2DServer.AreaGetParam(GetWorld2d().Space, Physics2DServer.AreaParameter.Gravity);
            GravityVector = (Vector2)Physics2DServer.AreaGetParam(GetWorld2d().Space, Physics2DServer.AreaParameter.GravityVector);

            if (InstigatorPath is not null)
            {
                Instigator = GetNode(InstigatorPath); 
            }

            ButtOffsetNode = GetNode<Position2D>(ButtPositionPath);
        }

        public override void _PhysicsProcess(Single delta)
        {
            Position += Velocity * delta;
            
            if (Held is false)
            {
                Velocity += GravityVector * GravityStrength * delta;
            }

            LookAt(GlobalPosition + Velocity);
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
                CollisionMask |= Core.Physics.Layers[Physics.Layer.Willie];
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
                CollisionMask = 0u;
            }
        }
    }
}
