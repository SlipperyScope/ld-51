using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace ld51.game
{
    public class Bouncer : StaticBody2D, IAudioTriggerable
    {
        private AudioStreamPlayer SFX;
        public override void _Ready()
        {
            SFX = GetNode<AudioStreamPlayer>("SFX");
        }

        public void TriggerAudio(Int32 index = 0)
        {
            switch (index)
            {
                case 0: SFX.Play(); break;
            }
        }
    }
}
