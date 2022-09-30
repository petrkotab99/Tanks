using ECSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks.Components
{
    public class HillPosition : IComponent
    {
        public Single Position { get; set; }

        public Int32 Lifes { get; set; }

        public HillPosition()
        {
            Lifes = 3;
        }
    }
}
