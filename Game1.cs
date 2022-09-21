using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Threading;

namespace TareaJuego
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Region donde se encuentran nuestras propiedades a utilizar
        #region Propiedades del juego 
        //Texturas del personaje y el fondo     
        Texture2D fondo;
        Texture2D nubes;
        Texture2D per;
        Texture2D perIz;
        Texture2D perDerecha;
        Texture2D perIzquierda;
        //*************************************************************
        //Propiedades encargadas de indicar el ancho y alto de la pantalla a mostrar
        int ancho;
        int alto;
        //Las siguientes propieades nos ayudaran a poder generar una textura con movimiento en nuestro fondo
        //rectan1 y rectan2 se encaragaran de mostrarnos la textura la cual basicamente se parte en 2 rectangulos
        Rectangle rectan1;
        Rectangle rectan2;
        int velocidad;//nos ayuda a establecer la velocidad con que queremos que se mueva las nubes que van en el fondo
        int rec1X;
        int rec2X;
        //****************************************************************
        //Posicion de la textura
        int perX = 0;
        int perY = 276;
        //Incremento de coordendas 
        int moverX = 0;
        int moverY = 0;
        int Direccion = 3;
        //*******************************************************************
        //Declaramos una fuente a usar
        private SpriteFont puntos;
        //Estableciendo un color para la fuente
        Color ColorFu = new Color(255, 249, 245);
        //*******************************************************************
        //Texturas enemigo
        Texture2D dra1;
        Texture2D dra2;
        //Variables que nos ayudaran con la posicion
        // int draX1 = 850;      
        //int draY = 5;
        //Se encarga de definir en que posicion saldra nuestro dragon y el tamaño de este 
        Rectangle dibuja;
        //Se encaraga de almacenar la velocidad en que se movera la textura
        Vector2 vel;
        //*********************************************************************
        //Texturas de puntos y daño 
        Texture2D pun;
        //Creamos una lista  para nuestros puntos
        List<Puntos> punto;
        //variables para poder controlar el tiempo
        TimeSpan tiempoPre;
        float puntoTime;//Con esta se indica el tiempo con que cae cada diamante
        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //tamaño ventana
            ancho = 1024;
            alto = 768;      
            this._graphics.PreferredBackBufferWidth = ancho;
            this._graphics.PreferredBackBufferHeight = alto;
        }

       

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Region que contiene las inicializaciones de algunas propiedades
            #region Inicializaciones y mas 
            //En rec1X y rec2X indicamos que tanto es lo que se estara recortando de la imagen establecida 
            rec1X = 0; // se inicializa desde 0 lo cual indica que aun no se a recortado nada
            //Creando un nuevo rectangulo desde la poscicion (0,0) indicando el ancho y alto de la pantalla
            rectan1 = new Rectangle(rec1X, 0, ancho, alto);

            //Especificando que rec2X tendra el valor de rec1X=0 + ancho de nuestra pantalla=1024
            rec2X = rec1X+rectan1.Width;
            //nuevo rectangulo desde la pocicion (1024,0)
            rectan2 = new Rectangle(rec2X, 0, ancho, alto);

            velocidad = 4;//especificamos la velocidad con que se desplazara cada parte
            //Para la generacion de puntos
            punto=new List<Puntos>();
            //Inicializando las variables de tiempo
            puntoTime = 3f;
            tiempoPre = TimeSpan.Zero;
            #endregion

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Region donde se encuentran cargadas las texturas 
            #region Cargando las texturas y fuente
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            //Cargando las texturas del personaje y fondo
            fondo = Content.Load<Texture2D>("Fondo");
            per = Content.Load<Texture2D>("TexDerecha");
            perIz = Content.Load<Texture2D>("TexIzquierda");
            nubes = Content.Load<Texture2D>("NubesFon");
            perDerecha = Content.Load<Texture2D>("Movimiento2");
            perIzquierda = Content.Load<Texture2D>("Movimiento1");
            //Cargando nuestra fuente
            puntos = Content.Load<SpriteFont>("Fuente");
            //Cargando texturas del enemigo
            dra1= Content.Load<Texture2D>("Dragon1");
            dra2 = Content.Load<Texture2D>("Dragon2");
            dibuja = new Rectangle(0, 5, dra1.Width, dra1.Height);
            //Asignando la velocidad;
            vel = new Vector2(3, 5);
            #endregion

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

         //Regin donde se encuentra algunas de las funbcionalidades del juego
            #region Animaciones movimientos y mas
            //Funciones que permiten que nuestras texturas se puedan mover
            KeyboardState currentState = Keyboard.GetState();
            Keys[] currenkey = currentState.GetPressedKeys();

            if (gameTime.TotalGameTime.Milliseconds % 50 == 0)
            {
                //Codigo que se ejecutara cada 20 milisegundos
                moverX = per.Width;
                moverY = per.Height;
            }
            else
            {
                moverX = 0;
                moverY = 0;
            }

            //Definimos los eventos de entrada para uso del jugador
            foreach (Keys key in currenkey)
            {
                if (key == Keys.Left)
                {
                    Direccion = 2;
                    perX = perX - moverX;

                }
                if (key == Keys.Right)
                {
                    Direccion = 3;
                    perX = perX + moverX;

                }
                if (key == Keys.Escape)
                {
                    this.Exit();
                }

                //Validando que la textura no se salga de los limites de la pantalla 
                if (perY < 0)
                    perY = 0;
                if (perX < 0)
                    perX = 0;
                if (perX + moverX > _graphics.GraphicsDevice.Viewport.Width)
                {
                    perX = _graphics.GraphicsDevice.Viewport.Width - moverX;
                }
                if (perY + moverY > _graphics.GraphicsDevice.Viewport.Height)
                {
                    perY = _graphics.GraphicsDevice.Viewport.Height - moverY;
                }

            }
            //********************************************************
            //Animacion personaje derecha
            if (gameTime.TotalGameTime.Milliseconds % 60 == 0)
            {
                per = Content.Load<Texture2D>("TexDerecha");
            }
            if (gameTime.TotalGameTime.Milliseconds % 220 == 0)
            {
                per = Content.Load<Texture2D>("Movimiento1");

            }
            //Animacion personaje izquiera
            if (gameTime.TotalGameTime.Milliseconds % 60 == 0)
            {
                perIz = Content.Load<Texture2D>("TexIzquierda");
            }
            if (gameTime.TotalGameTime.Milliseconds % 220 == 0)
            {
                perIz = Content.Load<Texture2D>("Movimiento2");
            }

            //********************************************************
            //Animacion movimiento de las nubes
            //restandole la velocidad a nuestras propiedades o el numero de pexiles establecidos  
            rec1X -= velocidad;
            rec2X -= velocidad;

            //Mostrando los nuevos rectagulos 
            rectan1 = new Rectangle(rec1X, -170, ancho, alto);
            rectan2 = new Rectangle(rec2X, -170, ancho, alto);

            //Usamos condicionales paran que caundo
            //el rectangulo 1 llega a menos el ancho establecido volvemos a empezar desde el inicio osea en 0
            if (rectan1.X == -ancho)
            {
                rec1X = 0;
            }
            //Cuando la posicion del rectagulo2 llega a 0 empezamos desde el valor establecido para el ancho  osea 1024
            if (rectan2.X == 0)
            {
                rec2X = ancho;
            }
            //********************************************************

            //Mueve el rectangulo de nuestra textura
            dibuja.X += (int)vel.X;
            //Nos permite que se de un efecto de rebote de nuestra textura en la pantalla
            if (dibuja.X + dibuja.Width > ancho || dibuja.X < 0)
            {
                //hacemos  basicamente que valla en la direccion contraria
                vel.X = -vel.X;
            }


            UpdatePuntos(gameTime);
            #endregion

            base.Update(gameTime);
        }

        //Region donde se encuentran algunos metodos implementados 
           #region Metodos implementados 
        //****************************************************************
        //Creando un metodo para los puntos
        private void UpdatePuntos(GameTime gameTime)
        {
            //En este metodo vamos generando mas diamantes 
            //Declaramos una variable la cual cargara los segundos que esten especificados en puntoTime
            TimeSpan time = TimeSpan.FromSeconds(puntoTime);

            if (gameTime.TotalGameTime - tiempoPre > time)
            {
                tiempoPre = gameTime.TotalGameTime;
                puntoTime *= 0.95f;//se encaraga disminuir el tiempo
                //Hcaemos un condicional que nos ayudara que no se generen tan rapido los diamantes
                if (puntoTime < 0.5f)              
                    puntoTime = 1.15f;            
                Addpunto(gameTime);
            }
            foreach(Puntos puntos in punto){
                puntos.Update(gameTime);
            }
        }

        //Creamos un metodo para adicionar los diamantes 
        private void Addpunto(GameTime gameTime)
        {
            //Creando un objeto ramdon que nos ayudara con la generacion de los diamantes
            Random ran = new Random();
            //Cargando texturas de los puntos y daño 
            pun = Content.Load<Texture2D>("diamantes");
            Rectangle punRectangle = new Rectangle(ran.Next(0,ancho-pun.Width), 0, pun.Width, pun.Height);
            Puntos p = new Puntos();
            p.Initialize(pun, punRectangle);
            //Pasando los diamantes a la lista
            punto.Add(p);
        }
        //*****************************************************************
        #endregion

        protected override void Draw(GameTime gameTime)
        {
        //Region donde se dibuja todo
           #region Dibujado las texturas y fuentes 
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //Dibujando las texturas, fondo y fuente utilizada
            _spriteBatch.Begin(); 
            _spriteBatch.Draw(fondo, new Rectangle(0, 0, ancho, alto), Color.White);
            // _spriteBatch.Draw(pun, new Rectangle(0,0,pun.Width,pun.Height), Color.White);
            _spriteBatch.Draw(nubes, rectan1, Color.White);
            _spriteBatch.Draw(nubes, rectan2, Color.White);   
            //***************************************************
            //Usamos condicionales para indicar la textura que debe dibujarse segun la tecla presionada
            if (Direccion == 3)
                {
                    _spriteBatch.Draw(per, new Rectangle(perX, perY, per.Width, per.Height), Color.White);
                }
                else if (Direccion == 2)
                {
                   _spriteBatch.Draw(perIz, new Rectangle(perX, perY, per.Width, per.Height), Color.White);
                }
            _spriteBatch.DrawString(puntos, "PUNTOS:", new Vector2(5, 6), ColorFu);
            //******************************************************
            //Dibujando las texturas del enemigo
            
                _spriteBatch.Draw(dra2, dibuja, Color.White);
            //_spriteBatch.Draw(dra1, dibuja, Color.White);

            DrawPunto(_spriteBatch);
            _spriteBatch.End();
            #endregion

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void DrawPunto(SpriteBatch spriteBatch)
        {
            foreach (Puntos puntos in punto)
            {
                puntos.Draw(spriteBatch);
            }
        }
    }
}