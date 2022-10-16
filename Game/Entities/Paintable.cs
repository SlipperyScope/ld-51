using Godot;
using ld51.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.Entities
{
    public class Paintable : Node2D, IPaintable
    {
        public void PaintDecal(Sprite decal, Transform2D transform)
        {
            if (decal.GetParent() is not null)
            {
                decal.Warn($"Decal still parented to {decal.GetParent().Name}");
            }
            else
            {
                AddChild(decal);
                decal.GlobalTransform = transform;
            }
        }
    }
}
