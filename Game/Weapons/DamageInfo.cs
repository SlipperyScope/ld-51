using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.Weapons
{
    /// <summary>
    /// Information about damage
    /// </summary>
    public record DamageInfo
    {
        /// <summary>
        /// Position damage occurred in global coordinates
        /// </summary>
        public Vector2 DamagePosition;

        /// <summary>
        /// Damage impulse (velocity * mass)
        /// </summary>
        public Vector2 Impulse;
    }
}
