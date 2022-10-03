using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.game
{
    public class Hood : RigidBody2D
    {
        private AnimationPlayer Anim;
        public Position2D SpawnPosition;

        public override void _Ready()
        {
            Anim = GetNode<AnimationPlayer>("AnimationPlayer");
            SpawnPosition = GetNode<Position2D>("Sprite/Spawn");
            SetAnim(0f);
        }

        public void SetAnim(Single time)
        {
            Anim.Play("Pull");
            Anim.Seek(time, true);
            Anim.Stop();
        }
    }
}
