#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Diagnostics;

#endregion

namespace Pathfinder
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {   
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //sprite texture for tiles, player, and ai bot
        Texture2D tile1Texture;
        Texture2D tile2Texture;
        Texture2D aiTexture;
        Texture2D playerTexture;

        //objects representing the level map, bot, and player 
        private Level level;
        private AiBotBase bot;
        private Player player;
        List<AiBotBase> allBots;
        

        //screen size and frame rate
        private const int TargetFrameRate = 50;
        private const int BackBufferWidth = 600;
        private const int BackBufferHeight = 600;

        public Game1()
        {
            //constructor
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = BackBufferHeight;
            graphics.PreferredBackBufferWidth = BackBufferWidth;
            Window.Title = "Pathfinder";
            Content.RootDirectory = "../../../Content";
            //set frame rate
            TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / TargetFrameRate);
            //load level map
            level = new Level();
            level.Loadmap("../../../Content/0.txt");
            //instantiate bot and player objects
            player = new Player(1, 1);
            //bot = new AiBotPathFollower(38, 38);
            //bot = new AiBotStatic(38, 38);

            allBots = new List<AiBotBase>() { new AiBotStatic(38, 38), new AiBotStatic(38, 15), new AiBotStatic(15, 38), new AiBotStatic(15,15), new AiBotStatic(5,5), new AiBotStatic(38,1), new AiBotStatic(1,38), new AiBotStatic(35,1), new AiBotStatic(1,35), new AiBotStatic(5,15), new AiBotStatic(15,5), new AiBotStatic(10,15), new AiBotStatic(15,10), new AiBotStatic(25,20), new AiBotStatic(20,25), new AiBotStatic(12,12), new AiBotStatic(1,8), new AiBotStatic(8,1), new AiBotStatic(32,18), new AiBotStatic(18,32)};

            //allBots = new List<AiBotBase>() { new AiBotStatic(38, 38), new AiBotStatic(38, 15), new AiBotStatic(15, 38), new AiBotStatic(15, 15), new AiBotStatic(5, 5), new AiBotStatic(38, 1), new AiBotStatic(1, 38), new AiBotStatic(35, 1), new AiBotStatic(1, 35), new AiBotStatic(5, 15), new AiBotStatic(15, 5), new AiBotStatic(10, 15), new AiBotStatic(15, 10), new AiBotStatic(25, 20), new AiBotStatic(20, 25), new AiBotStatic(12, 12), new AiBotStatic(1, 8), new AiBotStatic(8, 1), new AiBotStatic(32, 18), new AiBotStatic(18, 32) };

            foreach(AiBotBase b in allBots)
            {
                //b.Update(gameTime, level, player);
                b.Update(new GameTime(), level, player);
            }
            bot = allBots[0];

            DoTest();

        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //load the sprite textures
            Content.RootDirectory = "../../../Content";
            tile1Texture = Content.Load<Texture2D>("tile1");
            tile2Texture = Content.Load<Texture2D>("tile2");
            aiTexture = Content.Load<Texture2D>("ai");
            playerTexture = Content.Load<Texture2D>("target");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //player movement: read keyboard
            KeyboardState keyState = Keyboard.GetState();
            Coord2 currentPos = new Coord2();
            currentPos = player.GridPosition;
            if(keyState.IsKeyDown(Keys.Up))
            {
                currentPos.Y -= 1;
                player.SetNextLocation(currentPos, level);
                //level.aStar.Build(level, bot, player);
            }
            else if (keyState.IsKeyDown(Keys.Down))
            {
                currentPos.Y += 1;
                player.SetNextLocation(currentPos, level);
                //level.aStar.Build(level, bot, player);
            }
            else if (keyState.IsKeyDown(Keys.Left))
            {
                currentPos.X -= 1;
                player.SetNextLocation(currentPos, level);
                //level.aStar.Build(level, bot, player);
            }
            else if (keyState.IsKeyDown(Keys.Right))
            {
                currentPos.X += 1;
                player.SetNextLocation(currentPos, level);
                //level.aStar.Build(level, bot, player);
            }   

            if(keyState.IsKeyDown(Keys.P))
            {
                level.dijkstra.Build(level, bot, player);
            }

            //update bot and player
            bot.Update(gameTime, level, player);
            //allBots[1].Update(gameTime, level, player);
            player.Update(gameTime, level);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //draw level map
            DrawGrid();
            //draw bot
            foreach(AiBotBase b in allBots)
            {
                spriteBatch.Draw(aiTexture, b.ScreenPosition, Color.White);
            }
            //spriteBatch.Draw(aiTexture, bot.ScreenPosition, Color.White);
            //draw player
            spriteBatch.Draw(playerTexture, player.ScreenPosition, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawGrid()
        {
            //draws the map grid
            int sz = level.GridSize;
            for (int x = 0; x < sz; x++)
            {
                for (int y = 0; y < sz; y++)
                {
                    Coord2 pos = new Coord2((x*15), (y*15));

                    if(level.tiles[x,y] == 0)
                    {
                        if(level.aStar.inPath[x,y] || level.dijkstra.inPath[x,y])
                        {
                            spriteBatch.Draw(tile1Texture, pos, Color.Red);
                        }
                        else if (level.aStar.closed[x, y] || level.dijkstra.closed[x, y])
                        {
                            spriteBatch.Draw(tile1Texture, pos, Color.Blue);
                        }
                        else if(level.aStar.cost[x,y] < 100 || level.dijkstra.cost[x,y] < 100)
                        {
                            spriteBatch.Draw(tile1Texture, pos, Color.LightBlue);
                        }
                        else
                        {
                            spriteBatch.Draw(tile1Texture, pos, Color.White);
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(tile2Texture, pos, Color.White);
                    }

                    /*if (level.tiles[x, y] == 0)
                        spriteBatch.Draw(tile1Texture, pos, Color.White);
                    else 
                        spriteBatch.Draw(tile2Texture, pos, Color.White);*/
                }
            }
        }

        private void DoTest()
        {
            List<string> results = new List<string>();

            for (int algorithm = 0; algorithm < 4; algorithm++)
            {
                for (int i = 0; i < 3; i++)
                {
                    string resultsThisTime = "";
                    switch (algorithm)
                    {
                        case 0:
                            //dijkstra
                            resultsThisTime = i.ToString() + " map with Dijkstra,";
                            break;
                        case 1:
                            //A*
                            resultsThisTime = i.ToString() + " map with A*,";
                            break;
                        case 2:
                            //Pre-computed A*
                            resultsThisTime = i.ToString() + " map with pre-computed A*,";
                            break;
                        case 3:
                            //Scent
                            resultsThisTime = i.ToString() + " map with AvP Scent,";
                            break;
                    }
                    level.Loadmap("../../../Content/" + i.ToString() + ".txt");
                    for (int j = 0; j < 10; j++)
                    {
                        int time = 0;
                        switch(algorithm)
                        {
                            case 0:
                                //dijkstra
                                time = level.dijkstra.Build(level, bot, player);
                                resultsThisTime += time.ToString() + ",";
                                break;
                            case 1:
                                //A*
                                time = level.aStar.Build(level, bot, player);
                                resultsThisTime += time.ToString() + ",";
                                break;
                            case 2:
                                //Pre-computed A*
                                break;
                            case 3:
                                //Scent
                                break;
                        }
                        
                    }
                    results.Add(resultsThisTime);
                }
            }
            foreach (string s in results)
            {
                Debug.WriteLine(s);
            }

        }
    }
}
