using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using csharp_2048;

namespace csharp_2048
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D background;
        private Texture2D blueSqr;
        private Rectangle blueRect;
        private Texture2D _2;
        bool right = false;
        private Dictionary<int, Texture2D> blockTexture;
        Block kek;
        Board board;
        public static Random rnd = new Random();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 360;
            blueRect = new Rectangle(20, 20, 32, 32);
            Content.RootDirectory = "Content";
            this.Window.Title = "2048 by MCNH";
            blockTexture = new Dictionary<int, Texture2D>();
            board = new Board();
            kek = new csharp_2048.Block(1, 1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Block.spriteBatch = spriteBatch;
            background = Content.Load<Texture2D>("background");
            blueSqr = Content.Load<Texture2D>("blueSqr");
            //_2 = Content.Load<Texture2D>("newBlocks/block_256");
            for (int i = 2; i <= 2048; i = i * 2)
            {
                blockTexture.Add(i, Content.Load<Texture2D>("blocks/block_" + i));
            }
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            board.ControlKeyboard(Keyboard.GetState());
            // TODO: Add your update logic here
            if (right)
            {
                blueRect.X += 5;
                if (blueRect.X > 200)
                {
                    right = false;
                }
            }
            else
            {
                blueRect.X -= 5;
                if (blueRect.X < 10)
                {
                    right = true;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 360, 480), Color.White);
            spriteBatch.Draw(blueSqr, blueRect, Color.White);
            //  spriteBatch.Draw(blockTexture[512], new Rectangle(Board.offset + Block.size + Block.gap, Block.textureSize), Color.White);
            //kek.Draw(blockTexture[4], spriteBatch);
            // kek.Draw2(blockTexture[8]);
            board.drawBlocks(blockTexture);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
