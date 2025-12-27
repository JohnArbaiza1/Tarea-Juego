using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using TareaJuego.Personaje;
using static System.Formats.Asn1.AsnWriter;

namespace TareaJuego
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //************************************************************************************************
        //Region donde se encuentran nuestras propiedades a utilizar
        //************************************************************************************************

        #region Propiedades del juego 

        Texture2D fondo, nubes, spriteSheet, dragonTex, corazonTex, bolaTex, diamanteTex, gameover;
        SpriteFont font;
        Hero _hero; Dragon _dragon;
        List<BolaFuego> daños;
        List<Puntos> puntos;
        //--------------------------------------------------------------------------------------------
        //Propiedades encargadas de indicar el ancho y alto de la pantalla a mostrar
        int ancho;
        int alto;
        int rec1X = 0, rec2X, velocidad = 100;
        TimeSpan tiempoDmgAcc, tiempoPuntosAcc;
        int vidaMax = 5, vida, score;

        #endregion

        //*************************************************************

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //tamaño ventana
            ancho = 1592;
            alto = 1024;
            this._graphics.PreferredBackBufferWidth = ancho;
            this._graphics.PreferredBackBufferHeight = alto;
           
        }

        protected override void Initialize()
        {
            //Region que contiene las inicializaciones de algunas propiedades
            #region Inicializaciones y más
            daños = new List<BolaFuego>();
            puntos = new List<Puntos>();
            vida = vidaMax; score = 0;
            tiempoDmgAcc = tiempoPuntosAcc = TimeSpan.Zero;

            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Region donde se encuentran cargadas las texturas 
            #region Cargando las texturas y fuente

            //fondo del juego y nubes
            fondo = Content.Load<Texture2D>("fondo2");
            nubes = Content.Load<Texture2D>("NubesFon");
            // Cargando el spriteSheet del personaje y enemigo
            spriteSheet = Content.Load<Texture2D>("sprinte new");
            dragonTex = Content.Load<Texture2D>("dragon sprites_d");
            //Caraggando la parte de puntos y daños
            corazonTex = Content.Load<Texture2D>("corazon");
            bolaTex = Content.Load<Texture2D>("Bola de fuego1");
            diamanteTex = Content.Load<Texture2D>("diamante");
            gameover = Content.Load<Texture2D>("GAME OVER");
            font = Content.Load<SpriteFont>("Fuente");

            _hero = new Hero(spriteSheet);
            _dragon = new Dragon(dragonTex, new Vector2(300, 100));

            #endregion

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            // Parallax nubes
            rec1X = (rec1X - (int)(velocidad * gameTime.ElapsedGameTime.TotalSeconds) + ancho) % ancho;
            rec2X = (rec1X + ancho) % (2 * ancho);

            _hero.Update(gameTime); _dragon.Update(gameTime);

            GenerarObjetos(gameTime);
            ActualizarObjetos(gameTime);

            base.Update(gameTime);
        }

        private void GenerarObjetos(GameTime gt)
        {
            if (gt.TotalGameTime - tiempoDmgAcc > TimeSpan.FromSeconds(1))
            {
                tiempoDmgAcc = gt.TotalGameTime;
                AddBolaFuego();
            }
            if (gt.TotalGameTime - tiempoPuntosAcc > TimeSpan.FromSeconds(1.5))
            {
                tiempoPuntosAcc = gt.TotalGameTime;
                AddDiamante();
            }
        }

        private void ActualizarObjetos(GameTime gt)
        {
            for (int i = daños.Count - 1; i >= 0; i--)
            {
                daños[i].Update(gt);
                if (daños[i].Rectangle2.Y > alto) daños.RemoveAt(i);
                else if (_hero.BoundingBox.Intersects(daños[i].Rectangle2))
                {
                    vida--; daños.RemoveAt(i);
                }
            }

            for (int i = puntos.Count - 1; i >= 0; i--)
            {
                puntos[i].Update(gt);
                if (puntos[i].Rectangle.Y > alto) puntos.RemoveAt(i);
                else if (_hero.BoundingBox.Intersects(puntos[i].Rectangle))
                {
                    score++; puntos.RemoveAt(i);
                }
            }
        }

        private void AddBolaFuego()
        {
            var rand = new Random();
            Rectangle rect = new Rectangle(rand.Next(0, ancho - 50), 0, 50, 50);
            var b = new BolaFuego(); b.Initialize(bolaTex, rect);
            daños.Add(b);
        }

        private void AddDiamante()
        {
            var rand = new Random();
            Rectangle rect = new Rectangle(rand.Next(0, ancho - 32), 0, 32, 32);
            var p = new Puntos(); p.Initialize(diamanteTex, rect);
            puntos.Add(p);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(nubes, new Rectangle(rec1X, 0, ancho, alto), Color.White);
            _spriteBatch.Draw(nubes, new Rectangle((rec1X + ancho) % (2 * ancho), 0, ancho, alto), Color.White);
            _spriteBatch.Draw(fondo, new Rectangle(0, 0, ancho, alto), Color.White);

            if (vida > 0)
            {
                _hero.Draw(_spriteBatch);
                _dragon.Draw(_spriteBatch);

                foreach (var d in daños) d.Draw(_spriteBatch);
                foreach (var p in puntos) p.Draw(_spriteBatch);

                for (int i = 0; i < vida; i++)
                    _spriteBatch.Draw(corazonTex, new Rectangle(10 + i * 36, 10, 32, 32), Color.White);

                _spriteBatch.DrawString(font, $"Puntos: {score}", new Vector2(10, 50), Color.White);
            }
            else
            {
                _spriteBatch.Draw(gameover, new Rectangle(0, 0, ancho, alto), Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}