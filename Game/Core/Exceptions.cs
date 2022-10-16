using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld51.Core
{
    /// <summary>
    /// Generic exception that also prints to the console
    /// </summary>
    public class GodotException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GodotException"/> class with the specified error message
        /// </summary>
        /// <param name="message">Error message</param>
        public GodotException(String message = "") : base(message)
        {
            GD.PrintErr(message);
        }
    }
}
