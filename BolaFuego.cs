using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TareaJuego
{
    //Clase para las bolas de fuego
    internal class BolaFuego
    {
        //Definimos los atributos o propiedades que se necesitan 
        private Texture2D bola;//Nos pintara la bola de fuego 
        private Rectangle rec;

        //Definiendo el metodo Initialize en el cual pasamos los parametros que deseamos cargar
        public void Initialize(Texture2D bol, Rectangle rectangle)
        {
            this.bola = bol;
            this.rec = rectangle;
        }

        //Metodo Update
        public void Update(GameTime gameTime)
        {
            //ya que necesitamos uqe las bolas vayan callendo incrementamos a rec en el eje y
            rec.Y++;
        }

        //Metodo Draw 
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(bola, rec, Color.White);
        }


        public Rectangle Rectangle2
        {
            get { return rec; }
            set { rec = value; }
        }

    }
}
