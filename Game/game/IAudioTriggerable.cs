using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.game
{
    public interface IAudioTriggerable
    {
        public void TriggerAudio(Int32 index = 0);
    }
}
