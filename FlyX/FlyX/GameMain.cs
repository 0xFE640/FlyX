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
        private const int MaxLimit = 700;
        private readonly Random coord = new Random();
        private int offeset = 1;
        private const float koefSpeed = 100;

        private ArrayList Gaps { get; set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public SpriteFont Font { get; private set; }

        private bool hit;
        private MouseState currentMouseState, oldMouseState;
        private SpriteBatch spriteBatch;
        private Texture2D flyTexture, squashedTexture, aimTexture, gapTexture, barTexture;
        private Vector2 flyPosition, aimPosition;
        private Rectangle lifeBarPosition;
        private int lifePosition=100;
        

        private int scroll = 0;
        
        public GameMain()
        {
            new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = MaxLimit,
                PreferredBackBufferWidth = MaxLimit
            };

            // graphics.IsFullScreen = true;
            // IsMouseVisible = true;    
            Content.RootDirectory = "Content";
            flyPosition = new Vector2(50, 50);
            aimPosition = new Vector2();
            Gaps = new ArrayList();
            X = coord.Next(MaxLimit);
            Y = coord.Next(MaxLimit);
           
            
        }

        private void checkHit()
        {
            if (X > flyPosition.X)
                flyPosition.X += offeset;
            else
                if (X < flyPosition.X)
                    flyPosition.X -= offeset;

            if (X == flyPosition.X)
                X = coord.Next(MaxLimit);

            if (Y > flyPosition.Y)
                flyPosition.Y += offeset;
            else
                if (Y < flyPosition.Y)
                    flyPosition.Y -= offeset;

            if (Y == flyPosition.Y)
                Y = coord.Next(MaxLimit);

            if (currentMouseState.LeftButton == ButtonState.Pressed &&
                currentMouseState.X >= flyPosition.X &&
                currentMouseState.X <= flyPosition.X + flyTexture.Width &&
                currentMouseState.Y >= flyPosition.Y &&
                currentMouseState.Y <= flyPosition.Y + flyTexture.Height)
            {
                if (currentMouseState.LeftButton == ButtonState.Pressed &&
                    oldMouseState.LeftButton == ButtonState.Released)
                {
                    lifePosition -= 20;
                    if (lifePosition <= 0)
                    {
                        hit = true;
                        offeset = 0;
                    }
                }
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed &&
                oldMouseState.LeftButton == ButtonState.Released)
                Gaps.Add(new Vector2(currentMouseState.X, currentMouseState.Y));
            
        }
     
        // ReSharper disable once RedundantOverridenMember
        protected override void Initialize()
        {
            

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            flyTexture = Content.Load<Texture2D>(@"Images/Fly1");
            squashedTexture = Content.Load<Texture2D>(@"Images/squashed-fly");
            aimTexture = Content.Load<Texture2D>(@"Images/aim");
            gapTexture = Content.Load<Texture2D>(@"Images/gap");
            barTexture = Content.Load<Texture2D>(@"Images/bar");
            Font = Content.Load<SpriteFont>(@"Images/Font");

            
        }
      
        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
           TargetElapsedTime = TimeSpan.FromSeconds(1.0f /koefSpeed);
           
           oldMouseState = currentMouseState;
           currentMouseState = Mouse.GetState();
           aimPosition.X = currentMouseState.X;
           aimPosition.Y = currentMouseState.Y;
           scroll = 300; // currentMouseState.ScrollWheelValue;
           if (Keyboard.GetState().IsKeyDown(Keys.Escape))
               Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Back))
                lifePosition -= 1;


            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                lifePosition = 100;
                hit = false;
                offeset = 1;
            }
            checkHit();
            lifeBarPosition = new Rectangle((int)flyPosition.X, (int)flyPosition.Y + flyTexture.Height, lifePosition, 10); 

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSlateGray);

            spriteBatch.Begin();
            if (Gaps.Count > 0)
                foreach (Vector2 hole in Gaps) 
                    spriteBatch.Draw(gapTexture, hole, Color.White);
            if (!hit)
            {
                spriteBatch.Draw(flyTexture, flyPosition, Color.White);
                spriteBatch.Draw(barTexture, lifeBarPosition, Color.White);
            }
            //spriteBatch.DrawString(spriteFont, "x="+x.ToString()+"y="+y.ToString(), new Vector2(10, 10), Color.Black);
            else
            //spriteBatch.DrawString(spriteFont, "Bang", new Vector2(vector.X, vector.Y), Color.Black);
                spriteBatch.Draw(squashedTexture, flyPosition, Color.White);

            spriteBatch.Draw(aimTexture, aimPosition, Color.White);
            spriteBatch.DrawString(Font,lifePosition.ToString(), new Vector2(lifeBarPosition.X+40, lifeBarPosition.Y-2), Color.Black);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

//TODO: Red color when hit
/*
 * int counter = 1;
int limit = 50;
float countDuration = 2f; //every  2s.
float currentTime = 0f;

currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 

if (currentTime >= countDuration)
{
    counter++;
    currentTime -= countDuration; // "use up" the time
    //any actions to perform
}
if (counter >= limit)
{
    counter = 0;//Reset the counter;
    //any actions to perform
}
*/