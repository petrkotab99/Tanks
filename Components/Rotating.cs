using ECSL;

using System;
using System.Linq;


namespace Tanks.Components
{
    public class Rotating : IComponent
    {

        public Number Speed { get; set; }

        public Rotating()
        {
            Speed = 0;
        }

    }
}
