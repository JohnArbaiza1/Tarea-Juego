using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TareaJuego.Personaje
{
    internal class Animation
    {
        private readonly Texture2D _texture; // Sprite sheet de la animación
        private readonly List<Rectangle> _sourceRectangles = new(); // Lista de rectángulos que definen cada frame
        private readonly int _frames; // Cantidad total de frames en una fila
        private int frame; // Índice del frame actual
        private readonly float _frameTime; // Duración de cada frame (en segundos)
        private float _frameTimeLeft; // Tiempo restante para cambiar de frame
        private bool _active = true; // Si la animación está activa o no

        public Animation(Texture2D texture, int framesX, int framesY, float frameTime, int row = 1)
        {
            _texture = texture;
            _frameTime = frameTime;
            _frameTimeLeft = frameTime;
            _frames = framesX;
            // Calcula el ancho y alto de cada frame
            int width = _texture.Width / framesX;
            int height = _texture.Height / framesY;
            // Genera los rectángulos fuente para cada frame en la fila indicada
            for (int i = 0; i < _frames; i++)
            {
                _sourceRectangles.Add(new Rectangle(i * width, (row - 1) * height, width, height));
            }
        }

        public void Stop() => _active = false; // Pausa la animación
        public void Start() => _active = true; // Reanuda la animación

        public void Reset()
        {
            frame = 0;// Reinicia al primer frame
            _frameTimeLeft = _frameTime;
        }

        public void Update(GameTime gameTime)
        {
            if (!_active) return;
             // Resta el tiempo transcurrido desde el último frame
            _frameTimeLeft -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_frameTimeLeft <= 0f)
            {
                // Avanza al siguiente frame y reinicia el temporizador
                frame = (frame + 1) % _frames;
                _frameTimeLeft = _frameTime;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale = 1f, SpriteEffects effects = SpriteEffects.None)
        {
            spriteBatch.Draw(
                _texture,
                position,
                _sourceRectangles[frame],
                Color.White,
                0f,
                Vector2.Zero,
                scale,
                effects,
                0f
            );
        }

    }
}
