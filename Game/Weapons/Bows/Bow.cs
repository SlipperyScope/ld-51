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
        /// Fire strength at full draw
        /// </summary>
        [Export]
        public Single MaximumStrength { get; private set; }

        /// <summary>
        /// Current draw position [0, 1]
        /// </summary>
        public Single DrawPosition { get; set; }

        /// <summary>
        /// Current arrow instance
        /// </summary>
        private Arrow NockedArrow = null;
        
        /// <summary>
        /// Whether an arrow is nocked
        /// </summary>
        public Boolean ArrowIsNocked => NockedArrow is not null;

        /// <summary>
        /// Nocked <paramref name="arrow"/> into bow if one is not already nocked
        /// </summary>
        /// <param name="arrow">Arrow to nock</param>
        /// <returns>True if arrow was nocked</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Boolean NockArrow(Arrow arrow)
        {
            if (arrow is null) throw new ArgumentNullException();
            if (NockedArrow is not null) return false;
            NockedArrow = arrow;
            return true;
        }

        /// <summary>
        /// Fires the arrow
        /// </summary>
        public void FireArrow()
        {
            NockedArrow.Velocity += GlobalTransform.x * MaximumStrength * DrawPosition;
        }
    }
}
