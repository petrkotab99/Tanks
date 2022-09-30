using ECSL;
using ECSL.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks.Systems
{
    public class CloudBackgroundSystem : BaseSystem
    {

        private Dictionary<String, Entity> clouds;
        private Random random;

        public CloudBackgroundSystem(Engine engine, EntityPool pool) 
            : base(engine, pool)
        {
            clouds = new Dictionary<String, Entity>();
            random = new Random();
            for (int i = 0; i < 10; i++)
            {
                var entity = CreateCloud($"cloud{i}", random.Next(-512, 1979));
                clouds.Add(entity.Name, entity);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            HashSet<Entity> toRemove = new HashSet<Entity>();
            foreach (var entity in clouds.Values)
            {
                var physics = entity.GetComponent<Physics>();

                if (physics.Position.X > 1920)
                {
                    toRemove.Add(entity);
                    
                }
            }
            foreach (var entity in toRemove)
            {
                clouds.Remove(entity.Name);
                Pool.RemoveEntity(entity.Name);
                clouds.Add(entity.Name, CreateCloud(entity.Name, -512));
            }
        }

        private Entity CreateCloud(String name, Int32 xValue)
        {
            var entity = new Entity(name, Pool);

            var physics = new Physics()
            {
                Position = new Position(xValue, random.Next(-100, 100)),
                Speed = random.Next(1, 10) / 100f,
                Direction = new Position(1, 0),
            };
            var apperance = new Apperance()
            {
                TextureName = $"cloud{random.Next(1, 6)}",
                LayerDepth = 1,
            };
            if (apperance.TextureName == "cloud6")
                apperance.Scale = 0.15f;
            apperance.Position.SetRelativity(physics.Position);

            entity.AddComponent(physics);
            entity.AddComponent(apperance);

            return entity;
        }

    }
}
