using Microsoft.Xna.Framework;
using System;
using System.Linq;


namespace Tanks
{
    public class Pendulum
    {
        private Boolean up;

        public Single Current { get; private set; }

        public Single Max { get; set; }

        public Single Min { get; set; }

        public Single Original { get; private set; }

        public Single Step { get; set; }

        public Pendulum(Single min, Single max, Single original, Single step)
        {
            Original = original;
            Min = min;
            Max = max;
            Step = step;
        }

        public Single Update(GameTime gameTime, Single gameSpeed)
        {
            Single totalStep = Step * (Single)gameTime.ElapsedGameTime.TotalMilliseconds * gameSpeed;
            if (up)
            {
                Current += totalStep;
                if (Current + Original > Max)
                    up = false;
            }
            else
            {
                Current -= totalStep;
                if (Current + Original < Min)
                    up = true;
            }

            return Original + Current;
        }

    }
}
