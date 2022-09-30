using ECSL;
using ECSL.Systems;

using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Tanks.Systems;

namespace Tanks
{
    /// <summary>
    /// Represent a state.
    /// </summary>
    class LevelState : GameState
    {

        /// <summary>
        /// Initialize new state.
        /// </summary>
        /// <param name="engine">Local engine</param>
        public LevelState(Engine engine)
            : base(engine) { }

        /// <summary>
        /// Initialize level state.
        /// </summary>
        protected override void Initialize()
        {
            Pool.InputManager.AddContext("system");
            Pool.InputManager.AddContext("movement");

            Systems = new HashSet<BaseSystem>()
            {
                new ScallingSystem(Engine, Pool),
                new CloudBackgroundSystem(Engine, Pool),
                new PhysicsSystem(Engine, Pool),
                new RotatingSystem(Engine, Pool),
                new ShootingSystem(Engine, Pool),
                new MissileSystem(Engine, Pool),
                new DyingSystem(Engine, Pool),
            };
            RenderSystems = new HashSet<BaseSystem>()
            {
                new RenderSystem(Engine, Pool, Color.CornflowerBlue),
                new FuelRenderSystem(Engine, Pool),
                new ProgressSystem(Engine, Pool),
            };
            Factories = new HashSet<Factory>()
            {
                new LevelFactory(Engine, Pool),
            };

            base.Initialize();
        }

    }
}
