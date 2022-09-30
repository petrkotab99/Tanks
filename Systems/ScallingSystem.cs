using ECSL;
using ECSL.Components;

using Tanks.Components;

using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;


namespace Tanks.Systems
{
    public class ScallingSystem : GameSystem<Scalling, Apperance>
    {

        public Dictionary<String, Pendulum> Pendulums { get; set; }

        public ScallingSystem(Engine engine, EntityPool pool) 
            : base(engine, pool)
        {
            Pendulums = new Dictionary<String, Pendulum>();
        }

        protected override void Procces(Entity entity, GameTime gameTime, Scalling scalling, Apperance apperance)
        {
            if (!Pendulums.ContainsKey(entity.Name))
            {
                Pendulums.Add(entity.Name, new Pendulum(scalling.MinValue, scalling.MaxValue,
                    apperance.Scale.X, scalling.Step));
            }

            var pendulum = Pendulums[entity.Name];
            apperance.Scale = pendulum.Update(gameTime, Engine.GameSettings.GameSpeed);
        }
    }
}
