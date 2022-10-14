using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.Weapons
{
    public abstract class Bow : RigidBody2D
    {
        /// <summary>
        /// Path to string position node
        /// </summary>
        protected NodePath StringPositionPath = new("StringPosition");

        /// <summary>
        /// Path to animation player
        /// </summary>
        protected NodePath AnimatorPath = new("Animator");

        /// <summary>
        /// Bow draw animator
        /// </summary>
        protected AnimationPlayer Animator;

        /// <summary>
        /// Container to spawn arrows into
        /// </summary>
        [Export]
        private NodePath ArrowContainerPath;

        /// <summary>
        /// Fired arrow parent node
        /// </summary>
        protected Node2D ArrowContainer;

        /// <summary>
        /// Fire strength at full draw
        /// </summary>
        [Export]
        public Single MaximumStrength { get; private set; }

        /// <summary>
        /// Current draw position [0, 1]
        /// </summary>
        public Single DrawPosition
        {
            get => _DrawPosition;
            set
            {
                _DrawPosition = value;
                Animator.Play("Draw");
                Animator.Seek(value, true);
                Animator.Stop();
            }
        }
        public Single _DrawPosition = 0f;

        /// <summary>
        /// Current arrow instance
        /// </summary>
        private Arrow NockedArrow = null;
        
        /// <summary>
        /// Whether an arrow is nocked
        /// </summary>
        public Boolean ArrowIsNocked => NockedArrow is not null;

        protected Position2D StringPosition;

        public override void _Ready()
        {
            StringPosition = GetNode<Position2D>(StringPositionPath);
            Animator = GetNode<AnimationPlayer>(AnimatorPath);
            ArrowContainer = GetNode<Node2D>(ArrowContainerPath);
        }

        public override void _Process(Single delta)
        {
            var arrow = NockedArrow;
            if (arrow is not null)
            {
                arrow.GlobalTransform = StringPosition.GlobalTransform;
                arrow.Position -= arrow.Transform.x * arrow.ButtOffset.x + arrow.Transform.y * arrow.ButtOffset.y;
            }
        }

        /// <summary>
        /// Nocked <paramref name="arrow"/> into bow if one is not already nocked
        /// </summary>
        /// <param name="arrow">Arrow to nock</param>
        /// <returns>True if arrow was nocked</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Boolean NockArrow(Arrow arrow)
        {
            if (arrow is null) throw new ArgumentNullException();
            if (NockedArrow is not null)
            {
                GD.PushWarning("Tried to nock arrow when an arrow was already nocked");
                return false;
            }
            
            ArrowContainer.AddChild(arrow);
            arrow.Position -= arrow.ButtOffset;

            //arrow.Rotation = Rotation;
            //arrow.GlobalPosition = StringPosition.GlobalPosition - arrow.ButtOffset;
            
            NockedArrow = arrow;
            
            return true;
        }

        /// <summary>
        /// Fires the arrow
        /// </summary>
        public void FireArrow()
        {
            var arrow = NockedArrow;
            arrow.Velocity += arrow.Transform.x * MaximumStrength * DrawPosition;
            arrow.Held = false;
            NockedArrow = null;
            DrawPosition = 0f;
        }
    }
}
