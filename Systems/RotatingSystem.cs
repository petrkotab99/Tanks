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
    public class RotatingSystem : GameSystem<Rotating, Apperance>
    {
        public RotatingSystem(Engine engine, EntityPool pool)
            : base(engine, pool)
        {
        }

        protected override void Procces(Entity entity, GameTime gameTime,
            Rotating rotating, Apperance apperance)
        {
            apperance.Rotation += rotating.Speed.Value *
                gameTime.ElapsedGameTime.TotalMilliseconds *
                Engine.GameSettings.GameSpeed;

            if (apperance.Rotation > 2 * MathHelper.Pi)
                apperance.Rotation -= 2 * MathHelper.Pi;
            else if (apperance.Rotation > 0)
                apperance.Rotation += 2 * MathHelper.Pi;
        }
    }
}
