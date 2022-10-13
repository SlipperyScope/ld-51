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
        public void Damage(DamageInfo info);
    }
}
