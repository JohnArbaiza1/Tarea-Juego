using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TareaJuego
{
    internal class Puntos
    {
        private Texture2D _tex;
        private Rectangle _rect;
        private int _speed = 200;

        public void Initialize(Texture2D tex, Rectangle rect)
        {
            _tex = tex;
            _rect = rect;
        }

        public void Update(GameTime gameTime)
        {
            _rect.Y += (int)(_speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Draw(SpriteBatch sb)
            => sb.Draw(_tex, _rect, Color.White);

        public Rectangle Rectangle => _rect;
    }
}
