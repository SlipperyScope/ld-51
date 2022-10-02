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
        public void Touch(Vector2 at);
    }
}
