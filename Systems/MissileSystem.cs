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
    public class MissileSystem : GameSystem<Missile>
    {

        private const Single gravity = 0.006f;

        private HillGenerator hillGenerator;
        private Turn turn;

        public MissileSystem(Engine engine, EntityPool pool) 
            : base(engine, pool)
        {
            hillGenerator = HillGenerator.GetInstance();
            turn = Turn.GetInstance();
        }

        protected override void Procces(Entity entity, GameTime gameTime, Missile missile)
        {
            var physics = entity.GetComponent<Physics>();

            missile.Direction.Y += gravity;

            physics.Position.X.RawValue += missile.Direction.X * (Single)gameTime.ElapsedGameTime.TotalMilliseconds * Engine.GameSettings.GameSpeed;
            physics.Position.Y.RawValue += missile.Direction.Y * (Single)gameTime.ElapsedGameTime.TotalMilliseconds * Engine.GameSettings.GameSpeed;

            Rectangle rectangle = new Rectangle(0, 0, 1920, 1080);
            if (!rectangle.Contains(physics.Position.ToVector2()))
            {
                Pool.RemoveEntity("missile");
                turn.EndTurn();
                return;
            }

            if (hillGenerator.Collide(physics.Position, out int index))
            {
                Pool.RemoveEntity("missile");

                var entity1 = Pool.GetEntity("player1");
                var entity2 = Pool.GetEntity("player2");

                var physics1 = entity1.GetComponent<Physics>();
                var physics2 = entity2.GetComponent<Physics>();

                var hillPos1 = entity1.GetComponent<HillPosition>();
                var hillPos2 = entity2.GetComponent<HillPosition>();

                var newIndexes = hillGenerator.Booom(index, physics1.Position, physics2.Position, out int pIndex);

                hillPos1.Position = newIndexes.X;
                physics1.Position.X.RawValue = hillGenerator.GetPosition(hillPos1).X;
                physics1.Position.Y.RawValue = hillGenerator.GetPosition(hillPos1).Y;
                hillPos2.Position = newIndexes.Y;
                physics2.Position.X.RawValue = hillGenerator.GetPosition(hillPos2).X;
                physics2.Position.Y.RawValue = hillGenerator.GetPosition(hillPos2).Y;

                switch (pIndex)
                {
                    case 0:
                        var apperance1 = Pool.GetEntity("lifes1").GetComponent<Apperance>();
                        Int32 lifes1 = --Pool.GetEntity("player1").GetComponent<HillPosition>().Lifes;
                        apperance1.SourceRectangle = new Rectangle(0, 0, 140 * lifes1, 80);
                        Console.WriteLine("player1 damaged!");
                        break;
                    case 1:
                        var apperance2 = Pool.GetEntity("lifes2").GetComponent<Apperance>();
                        Int32 lifes2 = --Pool.GetEntity("player2").GetComponent<HillPosition>().Lifes;
                        apperance2.SourceRectangle = new Rectangle(0, 0, 140 * lifes2, 80);
                        Console.WriteLine("player2 damaged!");
                        break;
                }
                turn.EndTurn();
            }
        }

    }
}
