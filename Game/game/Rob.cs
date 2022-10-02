using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace ld51.game
{
    public class Rob : RigidBody2D, IGoal
    {
        public void Touch(Vector2 at)
        {
            CallDeferred(nameof(MakeRigid));
        }

        private void MakeRigid() => Mode = ModeEnum.Rigid;
    }
}
