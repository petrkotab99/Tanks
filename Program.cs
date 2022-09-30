using ECSL;

using System;
using System.Linq;


namespace Tanks
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //Game created 17.09.2018 19:23:18 in CLR v4.0.30319.42000 on DESKTOP-POKJPD6 by Petr.
        [STAThread]
        static void Main()
        {
            //Create local game engine
            using (var engine = new Engine())
            {
                //Set game state that will be active when game launch
                engine.SwitchState<LevelState>();
                //Launch the game
                engine.Run();
            }
        }
    }
#endif
}
