using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51
{

    public static class Core
    {
        public static Physics Physics { get; private set; } = new();
    }

    public record Physics
    {
        public enum Layer
        {
            None,
            Willie,
            ActiveArrow,
            Arrow,
        }

        public Dictionary<Layer, UInt32> Layers { get; private set; } = new();

        public Physics()
        {
            for (UInt32 i = 1; i <= 32; i++)
            {
                var name = ProjectSettings.GetSetting($"layer_names/2d_physics/layer_{i}");
                var layer = name switch
                {
                    "Willie" => Layer.Willie,
                    "ActiveArrow" => Layer.ActiveArrow,
                    "Arrow" => Layer.Arrow,
                    "Default" => Layer.None,
                    "Player" => Layer.None,
                    "Apple" => Layer.None,
                    "Wall" => Layer.None,
                    "" => Layer.None,
                    _ => throw new ApplicationException($"Physics layer [{name}]  not bound to enum"),
                };

                if (layer is not Layer.None)
                {
                    if (Layers.ContainsKey(layer))
                    {
                        //throw new ApplicationException($"Physics layer {name} (layer_{i}) is not unique");
                    }
                    else
                    Layers.Add(layer, 2u ^ (i - 1u));
                }
            }
        }
    }
}
