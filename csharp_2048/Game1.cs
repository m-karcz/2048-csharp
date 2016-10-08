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
        private Dictionary<int, Texture2D> blockTexture;
        Board board;
        SpriteFont font;
        public static Random rnd = new Random();
        private Texture2D win;
        private Texture2D newGameTexture;
        private Rectangle newGame;
        private bool lockMouse = false;
        private Rectangle cont;
        private Texture2D gameOver;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 360;
            Content.RootDirectory = "Content";
            this.Window.Title = "2048 by MCNH";
            blockTexture = new Dictionary<int, Texture2D>();
            Block.setTextures(blockTexture);
            newGame = new Rectangle(360 - 120 - 20, 30, 120, 60);
            cont = new Rectangle(90,215+120, 180, 60);
            board = new Board();
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
            this.IsMouseVisible = true;
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Block.spriteBatch = spriteBatch;
            background = Content.Load<Texture2D>("background");
            for (int i = 2; i <= 2048; i = i * 2)
            {
                blockTexture.Add(i, Content.Load<Texture2D>("blocks/block_" + i));
            }
            font = Content.Load<SpriteFont>("testfont");
            win = Content.Load<Texture2D>("youwon");
            newGameTexture = Content.Load<Texture2D>("newgame");
            gameOver = Content.Load<Texture2D>("gameover");
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
            board.ControlKeyboard();
            if(Mouse.GetState().LeftButton == ButtonState.Pressed &&
                newGame.Contains(Mouse.GetState().Position))
            {
                if (!lockMouse)
                {
                    board.Reset();
                    lockMouse = true;
                }
            }else
            {
                lockMouse = false;
            }
            if (board.toDraw==ToDraw.Win &&
                Mouse.GetState().LeftButton == ButtonState.Pressed &&
                cont.Contains(Mouse.GetState().Position))
            {
                board.toDraw = ToDraw.Nothing;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 360, 480), Color.White);
            board.DrawBlocks();
            spriteBatch.DrawString(font, "Score:", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font, board.GetPoints().ToString(), new Vector2(10, 50), Color.White);
            switch (board.toDraw)
            {
                case ToDraw.Win:
                    spriteBatch.Draw(win, new Rectangle(0, 120, 360, 360), Color.White);
                    break;
                case ToDraw.Lose:
                    spriteBatch.Draw(gameOver, new Rectangle(0, 120, 360, 360), Color.White);
                    break;
            }
            spriteBatch.Draw(newGameTexture, newGame, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
