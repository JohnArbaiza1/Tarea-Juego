using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Clase que nos sera de ayuda con la generacion de nuestros puntos
namespace TareaJuego
{
    internal class Puntos
    {
        private Texture2D tex;
        private Rectangle rectangle;

        //Definiendo el metodo Initialize en el cual pasamos los parametros que deseamos cargar
        public void Initialize(Texture2D tex, Rectangle rectangle)
        {
            this.tex = tex;
            this.rectangle = rectangle;
        }

        public void Update(GameTime gameTime)
        {
            //Incremetamos nuestro rectangulo en el eje y
            rectangle.Y++;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(tex,rectangle,Color.White);
        }
    }
}
