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
        private  float currentTime = 0f;
        private  float durationTime = 0f;
        private Color defaultFlyColor = Color.White;
        private const int displayWidth = 1024;
        private const int displayHeight = 768;
        private readonly Random coord = new Random();
        private int step = 1;
        private  float koefSpeed = 200;
        private const float delayHit = 0.20f;
        private int hitPoints = 100;
        private const int hitDamage = 10;

        private ArrayList Traces { get; set; }
        public int randX { get; private set; }
        public int randY { get; private set; }
        public SpriteFont Font { get; private set; }

        private bool isDead;
        private MouseState currentMouseState, oldMouseState;
        private SpriteBatch spriteBatch;
        private Texture2D flyTexture, squashedTexture, crosshairTexture, traceTexture, lifebarTexture;
        private Vector2 flyPosition, crosshairPosition;
        public Rectangle LifeBarPosition { get; private set; }

      //  private int scroll = 0;

        public GameMain()
        {
            new GraphicsDeviceManager(this)
            {
                    PreferredBackBufferHeight = displayHeight,
                    PreferredBackBufferWidth = displayWidth
            };
           
            Content.RootDirectory = "Content";
            flyPosition = new Vector2(50, 50);
            crosshairPosition = new Vector2();
            Traces = new ArrayList();
            randX = coord.Next(displayWidth);
            randY = coord.Next(displayHeight);
            // graphics.IsFullScreen = true;
            // IsMouseVisible = true;    
        
        }

        // ReSharper disable once RedundantOverridenMember
        protected override void Initialize()
        {

            base.Initialize();
        }
      
        private void checkHit( )
        {
            if (randX > flyPosition.X)
                flyPosition.X += step;
            else
                if (randX < flyPosition.X)
                    flyPosition.X -= step;

            if (randX == flyPosition.X)
                randX = coord.Next(displayWidth);

            if (randY > flyPosition.Y)
                flyPosition.Y += step;
            else
                if (randY < flyPosition.Y)
                    flyPosition.Y -= step;

            if (randY == flyPosition.Y)
                randY = coord.Next(displayHeight);

            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                if (currentMouseState.X >= flyPosition.X &&
                    currentMouseState.X < flyPosition.X + flyTexture.Width-50 &&
                    currentMouseState.Y >= flyPosition.Y-25 &&
                    currentMouseState.Y < flyPosition.Y-25 + flyTexture.Height)
                {
                    if (currentMouseState.LeftButton == ButtonState.Pressed &&
                        oldMouseState.LeftButton == ButtonState.Released)
                    {
                        defaultFlyColor = Color.Red;
                        if (durationTime == 0f)
                            durationTime = currentTime;

                        hitPoints -= hitDamage;
                        koefSpeed += 50;

                        if (hitPoints <= 0)
                        {
                            isDead = true;
                            step = 0;
                        }
                    }
                }
                else
                {
                    if (currentMouseState.LeftButton == ButtonState.Pressed &&
                        oldMouseState.LeftButton == ButtonState.Released)
                        Traces.Add(new Vector2(currentMouseState.X, currentMouseState.Y));
                }
            }

            if (currentTime > durationTime + delayHit)
            {
                defaultFlyColor = Color.White;
                durationTime = 0f;
            }

        }
     
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            flyTexture = Content.Load<Texture2D>(@"Images/Fly1");
            squashedTexture = Content.Load<Texture2D>(@"Images/squashed-fly");
            crosshairTexture = Content.Load<Texture2D>(@"Images/crosshair");
            traceTexture = Content.Load<Texture2D>(@"Images/gap");
            lifebarTexture = Content.Load<Texture2D>(@"Images/bar");
            Font = Content.Load<SpriteFont>(@"Images/Font");
            
        }
      
        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
          
           TargetElapsedTime = TimeSpan.FromSeconds(1.0f /koefSpeed); //Speed of Fly
           currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; 
           
           oldMouseState = currentMouseState;
           currentMouseState = Mouse.GetState();
           crosshairPosition.X = currentMouseState.X;
           crosshairPosition.Y = currentMouseState.Y;
           // = 300; // currentMouseState.ScrollWheelValue;
           if (Keyboard.GetState().IsKeyDown(Keys.Escape))
               Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                hitPoints = 100;
                isDead = false;
                step = 1;
            }
           checkHit();
           LifeBarPosition = new Rectangle((int)flyPosition.X, (int)flyPosition.Y + flyTexture.Height, hitPoints, 10); 

           base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            spriteBatch.Begin();
            if (Traces.Count > 0)
                foreach (Vector2 traces in Traces) 
                    spriteBatch.Draw(traceTexture, traces, Color.White);
            if (!isDead)
            {
                spriteBatch.Draw(flyTexture, flyPosition, defaultFlyColor);
                spriteBatch.Draw(lifebarTexture, LifeBarPosition, Color.Green);
            }
            //spriteBatch.DrawString(spriteFont, "x="+x.ToString()+"y="+y.ToString(), new Vector2(10, 10), Color.Black);
            else
                //spriteBatch.DrawString(spriteFont, "Bang", new Vector2(vector.randX, vector.randY), Color.Black);
                spriteBatch.Draw(squashedTexture, flyPosition, Color.White);

            spriteBatch.Draw(crosshairTexture, crosshairPosition, Color.White);
            spriteBatch.DrawString(Font,hitPoints.ToString(), new Vector2(LifeBarPosition.X+40, LifeBarPosition.Y-2), Color.Yellow);
            //spriteBatch.DrawString(Font, flyTexture.Width.ToString(),new Vector2(100,150), Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
    }
}
