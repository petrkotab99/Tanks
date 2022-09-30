using ECSL;
using ECSL.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks.Components;

namespace Tanks.Systems
{
    public class ShootingSystem : BaseSystem
    {

        private MouseState current;
        private MouseState last;
        private Turn turn;

        public ShootingSystem(Engine engine, EntityPool pool) 
            : base(engine, pool)
        {
            turn = Turn.GetInstance();
        }

        protected override void Update(GameTime gameTime)
        {
            last = current;
            current = Mouse.GetState();

            Rectangle rectangle = new Rectangle(0, 0, 1920, 1080);
            if (!rectangle.Contains(current.Position))
                return;

            if (current.LeftButton == ButtonState.Pressed)
            {
                turn.Power += 0.001 * (Single)gameTime.ElapsedGameTime.TotalMilliseconds * Engine.GameSettings.GameSpeed;
                if (turn.Power > turn.MaxPower)
                    turn.Power = turn.MaxPower;
            }
            if (last.LeftButton == ButtonState.Pressed && current.LeftButton == ButtonState.Released && Pool.IsNameAvaible("missile"))
            {
                Vector2 direction = new Vector2();
                if (turn.LeftPlayer)
                {
                    var physics = Pool.GetEntity("player1").GetComponent<Physics>();
                    direction = current.Position.ToVector2() - physics.Position.ToVector2();
                    CreateMissile(direction, physics.Position, gameTime, Engine);
                }
                else
                {
                    var physics = Pool.GetEntity("player2").GetComponent<Physics>();
                    direction = current.Position.ToVector2() - physics.Position.ToVector2();
                    CreateMissile(direction, physics.Position, gameTime, Engine);
                }
            }
        }

        private void CreateMissile(Vector2 direction, Vector2 position, GameTime gameTime, Engine engine)
        {
            var entity = new Entity("missile", Pool);

            var apperance = new Apperance()
            {
                TextureName = "missile",
                Scale = 0.25f,
                Origin = 128,
            };

            position.Y -= 20;

            var physics = new Physics()
            {
                Position = position,
            };

            Single total = Math.Abs(direction.X) + Math.Abs(direction.Y);

            var missile = new Missile()
            {
                Direction = new Position(direction.X / total * turn.Power, direction.Y / total * turn.Power),
            };

            entity.OnLoad += (sender, e) =>
            {
                apperance.Position.SetRelativity(physics.Position);
            };

            entity.AddComponent(physics);
            entity.AddComponent(missile);
            entity.AddComponent(apperance);

        }

    }
}
