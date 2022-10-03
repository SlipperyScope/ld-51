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
        private AudioStreamPlayer SFX2;

        public override void _Ready()
        {
            SFX = GetNode<AudioStreamPlayer>("SFX");
            SFX2 = GetNode<AudioStreamPlayer>("SFX2");
        }
        public void Touch(Vector2 at)
        {
            //CallDeferred(nameof(MakeRigid));
            GD.Print("OMGLIKEIMDEAD");
            SFX.Play();
        }

        public void Dangit()
        {
            SFX2.Play();
        }

        private void MakeRigid() => Mode = ModeEnum.Rigid;

        public void Kill() => GetParent<RobNHood>().Kill();
    }
}
