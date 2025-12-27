using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TareaJuego.Personaje
{
    internal class AnimationPersonaje
    {
        // Diccionario que asocia una clave (ej: "caminar", "saltar") con una animación
        private readonly Dictionary<object, Animation> _anims = new();
        // Guarda la última clave de animación reproducida (útil para el Draw)
        private object _lastKey;

        // Agrega una animación al diccionario, asociándola con una clave
        public void AddAnimation(object key, Animation anim)
        {
            _anims.Add(key, anim);
            _lastKey ??= key; // Si _lastKey aún no tiene valor, se le asigna esta primera clave
        }

        // Actualiza la animación correspondiente a la clave dada
        public void Update(object key, GameTime gameTime)
        {
            if (_anims.ContainsKey(key))
            {
                _anims[key].Start();
                _anims[key].Update(gameTime);
                _lastKey = key;
            }
            else
            {
                // Si la clave no existe, detiene y reinicia la última animación mostrada
                _anims[_lastKey].Stop();
                _anims[_lastKey].Reset();
            }
        }

        // Dibuja la animación actual (la última que se actualizó)
        public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale, SpriteEffects effects)
        {
            _anims[_lastKey].Draw(spriteBatch, position, scale, effects);
        }
    }
}
