using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ld51.Core;
using ld51.Utils;
using ld51.Entities;

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
        [Export]
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

        /// <summary>
        /// Gravity direction
        /// </summary>
        private Vector2 GravityVector;

        /// <summary>
        /// Gravity acceleration in u/s^2
        /// </summary>
        private Single GravityStrength;

        /// <summary>
        /// Maximum number of moves after collision
        /// </summary>
        const UInt32 MaxMoveAttempts = 5u;

        /// <summary>
        /// Current move attempt
        /// </summary>
        private UInt32 MoveAttemp = 0u;

        private UInt32 ActiveArrowLayer = Statics.Physics.Layers[Physics.Layer.ActiveArrow];

        /// <summary>
        /// Creates a new Arrow
        /// </summary>
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
            //Position += Velocity * delta;
            MoveAttemp = 0u;
            Move(Velocity * delta);

            if (Held is false)
            {
                Velocity += GravityVector * GravityStrength * delta;
            }

            LookAt(GlobalPosition + Velocity);
        }

        /// <summary>
        /// Processes a movement collision
        /// </summary>
        /// <param name="collision"></param>
        private void Move(Vector2 distance)
        {
            if (distance.LengthSquared() < 1f) return;
            if (MoveAttemp++ >= MaxMoveAttempts) return;

            var collision = MoveAndCollide(distance, false);
            if (collision is null) return;

            var removeAfterCollide = true;

            switch (collision.Collider)
            {
                case IDamageable damageable:
                    damageable.Damage(
                        new()
                        {
                            DamagePosition = collision.Position,
                            Impulse = Velocity
                        });
                    break;
            }

            if (removeAfterCollide is true)
            {
                if (collision.Collider is IPaintable paintable)
                {
                    var sprite = GetNode<Sprite>("Sprite");
                    var transform = sprite.GlobalTransform;
                    RemoveChild(sprite);
                    paintable.PaintDecal(sprite, transform);
                }
                QueueFree();
            }
        }

        /// <summary>
        /// Activates the arrow
        /// </summary>
        public virtual void Activate()
        {
            if (Active is false)
            {
                Active = true;
                CollisionLayer |= ActiveArrowLayer;
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
                CollisionLayer &= ~ActiveArrowLayer;
            }
        }
    }
}
