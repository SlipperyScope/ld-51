using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.Weapons
{
    /// <summary>
    /// A class that can be damaged
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Damage object
        /// </summary>
        /// <param name="info">Damage info</param>
        public void Damage(DamageInfo info);
    }
}
