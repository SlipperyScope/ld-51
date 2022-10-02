﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.game
{
    public class Apple : Prop, IGoal
    {
        public override void _EnterTree()
        {
            Disable();
        }

        public void Touch(Vector2 at)
        {
            GetNode<AudioStreamPlayer>("SFX").Play();
            GD.Print("Congradulations, you did it. Hooray.");
        }
    }
}
