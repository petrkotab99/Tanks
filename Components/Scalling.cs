using ECSL;

using System;
using System.Linq;


namespace Tanks.Components
{
    public class Scalling : IComponent
    {

        public Number MinValue { get; set; }

        public Number MaxValue { get; set; }

        public Number Step { get; set; }

        public Scalling()
        {
            MinValue = 0;
            MaxValue = 0;
            Step = 0;
        }

    }
}
