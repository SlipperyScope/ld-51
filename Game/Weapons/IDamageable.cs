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

    /// <summary>
    /// Handles damage taken events
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="e">Event args</param>
    public delegate void DamageTakenHandler(object sender, DamageTakenEventArgs e);

    /// <summary>
    /// Arguments for a damagetaken event
    /// </summary>
    public class DamageTakenEventArgs : EventArgs
    {
        /// <summary>
        /// Damage info
        /// </summary>
        public DamageInfo Info { get; private set; }

        /// <summary>
        /// Creates a new damage taken event args
        /// </summary>
        /// <param name="info">Damage info</param>
        public DamageTakenEventArgs(DamageInfo info)
        {
            Info = info;
        }
    }
}
