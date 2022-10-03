using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.game
{
    public class Gib : Prop, IGoal
    {
        public delegate void GibHitHandler(object source, GibHitEventArgs e);
        public event GibHitHandler GibHit;

        public void Touch(Vector2 at)
        {
            GibHit?.Invoke(this, new GibHitEventArgs(Enabled));
        }
    }

    public class GibHitEventArgs : EventArgs
    {
        public Boolean IsActive;

        public GibHitEventArgs(Boolean isActive)
        {
            IsActive = isActive;
        }
    }
}
