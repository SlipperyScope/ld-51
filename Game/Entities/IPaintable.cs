using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.Entities
{
    /// <summary>
    /// Entitiy that can be painted
    /// </summary>
    public interface IPaintable
    {
        /// <summary>
        /// Pains a decal onto the node
        /// </summary>
        /// <param name="decal"></param>
        /// <param name="transform">Global transform</param>
        public void PaintDecal(Sprite decal, Transform2D transform);
    }
}
