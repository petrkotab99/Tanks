using ECSL;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks.Systems
{
    class FuelRenderSystem : BaseSystem
    {


        private const String title = "Fuel: ";

        private Turn turn;
        private SpriteFont font;
        private SpriteBatch spriteBatch;

        public FuelRenderSystem(Engine engine, EntityPool pool)
            : base(engine, pool)
        {
            turn = Turn.GetInstance();
            LoadFont(engine);
            this.spriteBatch = new SpriteBatch(engine.GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            String text = title + turn.Fuel;
            spriteBatch.Begin();

            spriteBatch.DrawString(font, text, new Vector2(1920 / 2f - ((2 * font.MeasureString(title).X) / 3f), 950), Color.Black);

            spriteBatch.End();
        }

        private void LoadFont(Engine engine)
        {
            try
            {
                font = engine.Content.Load<SpriteFont>("Fonts/grinched");
            }
            catch
            {
                font = engine.Content.Load<SpriteFont>("Fonts/arial");
            }
        }

    }
}
