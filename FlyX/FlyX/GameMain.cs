using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;




// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable InconsistentNaming

namespace FlyX
{
    public class GameMain : Game
    {
        //private const int MaxLimit = 700;
        private readonly int displayWidth = 1024;
        private readonly int displayHeight = 768;
        private readonly Random coord = new Random();
        private int offeset = 1;
        private const float koefSpeed = 500.0f;

        private ArrayList Gaps { get; set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public SpriteFont Font { get; private set; }

        private bool hit;
        private MouseState currentMouseState, oldMouseState;
        private SpriteBatch spriteBatch;
        private Texture2D flyTexture, squashedTexture, aimTexture, gapTexture;
        private Vector2 flyPosition, aimPosition;
        private GraphicsDeviceManager graphics;
        
        public GameMain()
        {
           // IsMouseVisible = true;    
            graphics =
                new GraphicsDeviceManager(this)
                {
                    PreferredBackBufferHeight = displayHeight,
                    PreferredBackBufferWidth = displayWidth
                };


            Content.RootDirectory = "Content";
            flyPosition = new Vector2(50, 50);
            aimPosition = new Vector2();
            Gaps = new ArrayList();
        }
     
        protected override void Initialize()
        {
            X = coord.Next(displayWidth);
            Y = coord.Next(displayHeight);
            
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            flyTexture = Content.Load<Texture2D>(@"Images/Fly1");
            squashedTexture = Content.Load<Texture2D>(@"Images/squashed-fly");
            aimTexture = Content.Load<Texture2D>(@"Images/aim");
            gapTexture = Content.Load<Texture2D>(@"Images/gap");
            Font = Content.Load<SpriteFont>(@"Images/Font");
        }
      
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
           oldMouseState = currentMouseState;
           currentMouseState = Mouse.GetState();
           aimPosition.X = currentMouseState.X;
           aimPosition.Y = currentMouseState.Y;
            if (Keyboard.GetState().IsKeyDown(Keys.F))
                
               // graphics.IsFullScreen = true;;
           if (Keyboard.GetState().IsKeyDown(Keys.Escape))
               Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                hit = false;
                offeset = 1;
            }
            

            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / koefSpeed);

            if (X > flyPosition.X)
                flyPosition.X += offeset;
            else
                if (X < flyPosition.X)
                    flyPosition.X -= offeset;

            if (X == flyPosition.X)
                X = coord.Next(displayWidth);

            if (Y > flyPosition.Y)
                flyPosition.Y += offeset;
            else
                if (Y < flyPosition.Y)
                    flyPosition.Y -= offeset;

            if (Y == flyPosition.Y)
                Y = coord.Next(displayHeight);

            if (currentMouseState.LeftButton == ButtonState.Pressed &&
                currentMouseState.X >= flyPosition.X &&
                currentMouseState.X <= flyPosition.X + flyTexture.Width &&
                currentMouseState.Y >= flyPosition.Y &&
                currentMouseState.Y <= flyPosition.Y + flyTexture.Height)
            {
             //   Window.Title = currentMouseState.X + " " + currentMouseState.Y;
               hit = true;
               offeset = 0;
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed && 
                oldMouseState.LeftButton == ButtonState.Released) 
                Gaps.Add(new Vector2(currentMouseState.X, currentMouseState.Y));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.BlueViolet);
            spriteBatch.Begin(/*SpriteSortMode.Texture, BlendState.AlphaBlend*/);
            
            // if (currentMouseState.LeftButton == ButtonState.Pressed)
            // spriteBatch.Draw(squashedTexture, new Vector2(currentMouseState.X,currentMouseState.Y), Color.White);

            if (Gaps.Count > 0)
                foreach (Vector2 hole in Gaps) 
                    spriteBatch.Draw(gapTexture, hole, Color.White);

            if (!hit)
                spriteBatch.Draw(flyTexture, flyPosition, Color.White);
            //spriteBatch.DrawString(spriteFont, "x="+x.ToString()+"y="+y.ToString(), new Vector2(10, 10), Color.Black);
            else
                //spriteBatch.DrawString(spriteFont, "Bang", new Vector2(vector.X, vector.Y), Color.Black);
                spriteBatch.Draw(squashedTexture, flyPosition, Color.White);

            spriteBatch.Draw(aimTexture, aimPosition, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
