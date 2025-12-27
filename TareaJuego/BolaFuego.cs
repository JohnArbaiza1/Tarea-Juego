using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TareaJuego
{
    internal class BolaFuego
    {
        private Texture2D _texture;
        private Rectangle _rect;
        private int _speed = 250;

        public void Initialize(Texture2D texture, Rectangle rect)
        {
            _texture = texture;
            _rect = rect;
        }

        public void Update(GameTime gameTime)
        {
            _rect.Y += (int)(_speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Draw(SpriteBatch sb)
            => sb.Draw(_texture, _rect, Color.White);

        public Rectangle Rectangle2 => _rect;
    }
}
