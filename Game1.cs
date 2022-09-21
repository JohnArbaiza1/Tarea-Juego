using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Threading;

namespace TareaJuego
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        

        //Texturas del personaje y el fondo     
        Texture2D fondo;
        Texture2D nubes ;
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

            //En rec1X y rec2X indicamos que tanto es lo que se estara recortando de la imagen establecida 
            rec1X = 0; // se inicializa desde 0 lo cual indica que aun no se a recortado nada
            //Creando un nuevo rectangulo desde la poscicion (0,0) indicando el ancho y alto de la pantalla
            rectan1 = new Rectangle(rec1X, 0, ancho, alto);

            //Especificando que rec2X tendra el valor de rec1X=0 + ancho de nuestra pantalla=1024
            rec2X = rec1X+rectan1.Width;
            //nuevo rectangulo desde la pocicion (1024,0)
            rectan2 = new Rectangle(rec2X, 0, ancho, alto);

            velocidad = 4;//especificamos la velocidad con que se desplazara cada parte   
            base.Initialize();
        }

        protected override void LoadContent()
        {
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



            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

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
                if(key == Keys.Escape)
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
            //restandole la velocidad a nuestras propiedades o el numero de pexiles establecidos  
            rec1X -= velocidad;
            rec2X -= velocidad;

            //Mostrando los nuevos rectagulos 
            rectan1=new Rectangle(rec1X, -170, ancho, alto);
            rectan2=new Rectangle(rec2X, -170, ancho, alto);

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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //Dibujando las texturas, fondo y fuente utilizada
            _spriteBatch.Begin(); 
            _spriteBatch.Draw(fondo, new Rectangle(0, 0, ancho, alto), Color.White);    
            _spriteBatch.Draw(nubes, rectan1, Color.White);
            _spriteBatch.Draw(nubes, rectan2, Color.White);

            //Usamos condicionales para indicar la textura que debe dibujarse segun la tecla presionada
            if (Direccion == 3)
                {
                    _spriteBatch.Draw(per, new Rectangle(perX, perY, per.Width, per.Height), Color.White);
                }
                else if (Direccion == 2)
                {
                   _spriteBatch.Draw(perIz, new Rectangle(perX, perY, per.Width, per.Height), Color.White);
                }

            _spriteBatch.DrawString(puntos, "PUNTOS:", new Vector2(5, 6),ColorFu);

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}