using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TareaJuego.Personaje
{
    internal class Dragon
    {
        private Vector2 _position;
        private float _baseY;
        private float _speed = 180f;                         // Velocidad de movimiento horizontal
        private float _waveAmplitude = 20f;                  // Amplitud del movimiento ondulante
        private float _waveTime = 0f;                        // Tiempo acumulado para onda senoidal
        private SpriteEffects _spriteEffect = SpriteEffects.None; // Para voltear el sprite según la dirección
        private readonly AnimationPersonaje _animation = new();

        public Dragon(Texture2D texture, Vector2 startPosition)
        {
            _position = startPosition;
            _baseY = startPosition.Y;

            _animation.AddAnimation("volando", new Animation(texture, 3, 1, 0.8f, 1)); // fila 1 = volando
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Movimiento horizontal automático
            _position.X += _speed * elapsed;

            // Movimiento ondulante (basado en _baseY)
            _waveTime += elapsed;
            _position.Y = _baseY + (float)Math.Sin(_waveTime * 2f) * _waveAmplitude;

            // Rebote al llegar a los bordes y cambio de dirección con flip visual
            if (_position.X >= 1208) // Límite derecho (ajústalo al tamaño de tu ventana)
            {
                _speed = -Math.Abs(_speed); // Cambia a izquierda
                _spriteEffect = SpriteEffects.FlipHorizontally; // Voltea el sprite
            }
            else if (_position.X <= 10) // Límite izquierdo
            {
                _speed = Math.Abs(_speed); // Cambia a derecha
                _spriteEffect = SpriteEffects.None; // Restaura orientación normal
            }

            _animation.Update("volando", gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Dibuja el dragón con la animación actual y efecto de orientación
            _animation.Draw(spriteBatch, _position, 1f, _spriteEffect);
        }
    }
}

