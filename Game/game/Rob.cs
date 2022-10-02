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
        private AudioStreamPlayer SFX;
        public override void _Ready()
        {
            SFX = GetNode<AudioStreamPlayer>("SFX");

        }
        public void Touch(Vector2 at)
        {
            CallDeferred(nameof(MakeRigid));
            GD.Print("OMGLIKEIMDEAD");
            SFX.Play();
        }

        private void MakeRigid() => Mode = ModeEnum.Rigid;
    }
}
