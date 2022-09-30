using ECSL;
using ECSL.Components;
using ECSL.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks.Components;

namespace Tanks.Scripts
{
    public class MovementScript : InputScript
    {

        private const Single speed = 0.25f;

        private MovementState movementState1;
        private MovementState movementState2;
        private Turn turn;
        private HillGenerator hillGenerator;

        public MovementScript(Engine engine) 
            : base(engine, "movement")
        {
            turn = Turn.GetInstance();
            hillGenerator = HillGenerator.GetInstance();
        }

        public override void Procces(EntityPool pool, GameTime gameTime)
        {
            if (turn.Fuel == 0)
                return;

            var entity1 = pool.GetEntity("player1");
            var physics1 = entity1.GetComponent<Physics>();
            var hillPosition1 = entity1.GetComponent<HillPosition>();

            var entity2 = pool.GetEntity("player2");
            var physics2 = entity2.GetComponent<Physics>();
            var hillPosition2 = entity2.GetComponent<HillPosition>();

            Single total = 0;

            if (turn.LeftPlayer)
            {
                switch (movementState1)
                {
                    case MovementState.Stay:
                        if (InputContext.IsKeyDown("moveRight", true) && InputContext.IsKeyUp("moveLeft", true))
                            movementState1 = MovementState.Left;
                        if (InputContext.IsKeyDown("moveLeft", true) && InputContext.IsKeyUp("moveRight", true))
                            movementState1 = MovementState.Right;
                        break;
                    case MovementState.Left:
                        if (InputContext.IsKeyUp("moveLeft", true) || InputContext.IsKeyDown("moveRight", true))
                            movementState1 = MovementState.Stay;

                        total = speed * (Single)gameTime.ElapsedGameTime.TotalMilliseconds * Engine.GameSettings.GameSpeed;
                        break;
                    case MovementState.Right:
                        if (InputContext.IsKeyUp("moveRight", true) || InputContext.IsKeyDown("moveLeft", true))
                            movementState1 = MovementState.Stay;

                        total = -speed * (Single)gameTime.ElapsedGameTime.TotalMilliseconds * Engine.GameSettings.GameSpeed;
                        break;
                }

                hillPosition1.Position += total;
                UpdateStats(physics1, hillPosition1);
            }
            else
            {
                switch (movementState2)
                {
                    case MovementState.Stay:
                        if (InputContext.IsKeyDown("moveRight", true) && InputContext.IsKeyUp("moveLeft", true))
                            movementState2 = MovementState.Left;
                        if (InputContext.IsKeyDown("moveLeft", true) && InputContext.IsKeyUp("moveRight", true))
                            movementState2 = MovementState.Right;
                        break;
                    case MovementState.Left:
                        if (InputContext.IsKeyUp("moveLeft", true) || InputContext.IsKeyDown("moveRight", true))
                            movementState2 = MovementState.Stay;

                        total = speed * (Single)gameTime.ElapsedGameTime.TotalMilliseconds * Engine.GameSettings.GameSpeed;
                        break;
                    case MovementState.Right:
                        if (InputContext.IsKeyUp("moveRight", true) || InputContext.IsKeyDown("moveLeft", true))
                            movementState2 = MovementState.Stay;

                        total = -speed * (Single)gameTime.ElapsedGameTime.TotalMilliseconds * Engine.GameSettings.GameSpeed;
                        break;
                }

                hillPosition2.Position += total;
                UpdateStats(physics2, hillPosition2);
            }

            turn.Fuel -= (Int32)Math.Abs(total);
            if (turn.Fuel < 0)
                turn.Fuel = 0;
        }

        private void UpdateStats(Physics physics, HillPosition hillPosition)
        {
            Position position = hillGenerator.GetPosition(hillPosition);
            physics.Position.X.RawValue = position.X;
            physics.Position.Y.RawValue = position.Y;
        }

    }
}
