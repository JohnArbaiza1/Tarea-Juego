using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TareaJuego.Personaje
{
    internal class Hero
    {
        private Vector2 _position = new Vector2(90, 800); // Posición actual del héroe en la pantalla
        private readonly float _speed = 200f; // Velocidad de movimiento en píxeles por segundo
        private readonly AnimationPersonaje _animation = new(); // Objeto que maneja las animaciones del personaje
        private SpriteEffects _spriteEffect = SpriteEffects.None; // Flip visual según dirección
        private int _width = 80, _height = 100;

        // Constructor: inicializa las animaciones del personaje
        public Hero(Texture2D texture)
        {
            _animation.AddAnimation("quieto", new Animation(texture, 4, 3, 0.9f, 1)); // fila 1 (quieto)
            _animation.AddAnimation("derecha", new Animation(texture, 4, 3, 0.1f, 2)); // fila 2 caminando a la derecha
            _animation.AddAnimation("izquierda", new Animation(texture, 4, 3, 0.1f, 3)); // fila 3 caminando a la izquierda
        }

        public void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            string anim = "quieto";

            if (kstate.IsKeyDown(Keys.Right))
            {
                _position.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                anim = "derecha";
            }
            else if (kstate.IsKeyDown(Keys.Left))
            {
                _position.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                anim = "izquierda";
            }

            _animation.Update(anim, gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Dibuja la animación actual, aplicando el efecto visual (flip)
            _animation.Draw(spriteBatch, _position, 0.5f, _spriteEffect);
        }

        //Parte donde se trabaja la Colisión
        public Rectangle BoundingBox =>
            new Rectangle((int)_position.X, (int)_position.Y, _width, _height);
    }
}
