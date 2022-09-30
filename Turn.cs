using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public class Turn
    {

        private const Int32 maxFuel = 200;

        private static Turn instance;

        public Boolean LeftPlayer { get; private set; }

        public Int32 Fuel { get; set; }

        public Single Power { get; set; }

        public Single MaxPower { get; private set; }

        private Turn()
        {
            LeftPlayer = true;
            Fuel = maxFuel;
            MaxPower = 1.2f;
        }

        public static Turn GetInstance()
        {
            if (instance == null)
                instance = new Turn();

            return instance;
        }

        public void EndTurn()
        {
            LeftPlayer = !LeftPlayer;
            Fuel = maxFuel;
            Power = 0;
            if (LeftPlayer)
                Console.WriteLine("player1 turn!");
            else
                Console.WriteLine("player2 turn!");
        }

    }
}
