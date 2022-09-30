using ECSL;
using ECSL.Components;

using System;
using System.Linq;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ECSL.Managers;
using Tanks.Components;

namespace Tanks
{
    /// <summary>
    /// Represent a factory.
    /// </summary>
    class LevelFactory : Factory
    {

        /// <summary>
        /// Initialize new factory.
        /// </summary>
        /// <param name="engine">Local engine</param>
        /// <param name="pool">Local entity pool</param>
        public LevelFactory(Engine engine, EntityPool pool)
            : base(engine, pool)
        {

        }

        /// <summary>
        /// Initialize factory
        /// </summary>
        protected override void Initialize()
        {
            HillGenerator hillGenerator = HillGenerator.GetInstance();
            var hills = hillGenerator.GenerateHills(Engine.GraphicsDevice,
                600, 900, 120, 1920, 1080);
            TextureManager.AddTexture(hills, "hills");
        }

        /// <summary>
        /// Craft entities from components
        /// </summary>
        protected override void Craft()
        {
            FileManager.LoadEntity("Content/Entities/Level/hills", Pool, "hills");
            FileManager.LoadEntity("Content/Entities/Level/sun", Pool, "sun");
            FileManager.LoadEntity("Content/Entities/Level/bar", Pool, "bar");
            FileManager.LoadEntity("Content/Entities/Level/progress", Pool, "progress");
            FileManager.LoadEntity("Content/Entities/Level/lifes1", Pool, "lifes1");
            FileManager.LoadEntity("Content/Entities/Level/lifes2", Pool, "lifes2");

            CraftPlayer("player1", new Color(22, 80, 173), true);
            CraftPlayer("player2", new Color(140, 9, 9), false);
        }

        private void CraftPlayer(String name, Color color, Boolean left)
        {
            var entity = 
                FileManager.LoadEntity("Content/Entities/Level/player", Pool, name);

            entity.OnLoad += (sender, e) =>
            {
                var hillsGenerator = HillGenerator.GetInstance();

                var apperance = entity.GetComponent<Apperance>();
                var physics = entity.GetComponent<Physics>();
                var hillPosition = entity.GetComponent<HillPosition>();

                apperance.Color = color;

                hillPosition.Position = hillsGenerator.GetSpawnPosition(left);
                var position = hillsGenerator.GetPosition(hillPosition);

                physics.Position.X.RawValue = position.X;
                physics.Position.Y.RawValue = position.Y;
            };
        }

    }
}
