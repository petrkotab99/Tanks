using ECSL;
using ECSL.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks.Components;

namespace Tanks.Systems
{
    public class ProgressSystem : GameSystem<Progress, Apperance>
    {

        private Turn turn;

        public ProgressSystem(Engine engine, EntityPool pool)
            : base(engine, pool)
        {
            turn = Turn.GetInstance();
        }

        protected override void Procces(Entity entity, GameTime gameTime,
            Progress progress, Apperance apperance)
        {
            Single part = 420 / turn.MaxPower;
            Single value = turn.Power * part;
            apperance.SourceRectangle = new Rectangle(0, 0, (Int32)value, 80);
        }
    }
}
