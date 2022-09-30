using ECSL;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tanks.Components;

namespace Tanks.Systems
{
    class DyingSystem : GameSystem<HillPosition>
    {
        public DyingSystem(Engine engine, EntityPool pool) 
            : base(engine, pool)
        {
        }

        protected override void Procces(Entity entity, GameTime gameTime, HillPosition hillPosition)
        {
            if (hillPosition.Lifes <= 0)
            {
                MessageBox.Show((entity.Name == "player1" ? "Red" : "Blue") + " player win!", "GAME OVER!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Engine.Exit();
            }
        }
    }
}
