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

        [Export]
        public Boolean Bleeds = true;

        public void Touch(TouchData data)
        {
            GibHit?.Invoke(this, new GibHitEventArgs(Enabled, data));
        }
    }

    public class GibHitEventArgs : EventArgs
    {
        public Boolean IsActive;
        public TouchData Data;
        public GibHitEventArgs(Boolean isActive, TouchData data)
        {
            IsActive = isActive;
            Data = data;
        }
    }
}
