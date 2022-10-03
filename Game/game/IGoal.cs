using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.game
{
    public interface IGoal
    {
        /// <summary>
        /// Touch goal
        /// </summary>
        /// <param name="at">Position in global coordinates</param>
        public void Touch(TouchData data);
    }

    public record TouchData
    {
        public Vector2 Location;
        public Vector2 Direction;
        public Vector2 Velocity;
    }
}
