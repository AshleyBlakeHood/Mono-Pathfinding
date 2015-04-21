using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Pathfinder
{
    class Dijkstra
    {
        public bool[,] closed; //whether or not location is closed
        public float[,] cost; //cost values for each location
        public Coord2[,] link; //link for each location == coords of neighbouring location
        public bool[,] inPath; //whether or not a location is in the final path

        float[,] costings = new float[3, 3] { { 1.4f, 1.0f, 1.4f }, { 1.0f, 0.0f, 1.0f }, { 1.4f, 1.0f, 1.4f } };

        public Dijkstra()
        {
            closed = new bool[40, 40];
            cost = new float[40, 40];
            link = new Coord2[40, 40];
            inPath = new bool[40, 40];
        }

        public int Build(Level level, AiBotBase bot, Player plr)
        {
            DateTime start = DateTime.Now;
            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 40; y++)
                {
                    closed[x, y] = false;
                    cost[x, y] = 1000000.0f;
                    link[x, y] = new Coord2(-1, -1);
                    inPath[x, y] = false;
                }
            }

            cost[bot.GridPosition.X, bot.GridPosition.Y] = 0.0f;
            bool done = false;
            Coord2 bestPos = new Coord2(0, 0);
            while (!done)
            {
                float checkCost = 1000000;
                for (int x = 0; x < 40; x++)
                {
                    for (int y = 0; y < 40; y++)
                    {
                        if (cost[x, y] < checkCost && !closed[x, y])
                        {
                            bestPos.X = x;
                            bestPos.Y = y;
                            checkCost = cost[x, y];
                            //Debug.WriteLine("Best Pos X: " + bestPos.X + "   Y:" + bestPos.Y);
                        }
                    }
                }

                //Debug.WriteLine(plr.GridPosition.X + " " + plr.GridPosition.Y + "::" + bot.GridPosition.X + " " + bot.GridPosition.Y);
                closed[bestPos.X, bestPos.Y] = true;
                // For some reason this code is broken? Not sure why.
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        float costMod = costings[x + 1, y + 1];
                        float newCost = cost[bestPos.X, bestPos.Y] + costMod;
                        Coord2 checkPos = new Coord2(bestPos.X + x, bestPos.Y + y);
                        if (level.ValidPosition(checkPos))
                        {
                            if(cost[bestPos.X + x, bestPos.Y + y] > newCost)
                            {
                                cost[bestPos.X + x, bestPos.Y + y] = newCost;
                                link[bestPos.X + x, bestPos.Y + y].X = bestPos.X;
                                link[bestPos.X + x, bestPos.Y + y].Y = bestPos.Y;
                            }
                        }

                        /*try
                        //{
                            if (bestPos.X > 0 && bestPos.Y > 0 && bestPos.X < 39 && bestPos.Y < 39)
                            {
                                if (cost[bestPos.X + x, bestPos.Y + y] > newCost && level.ValidPosition(bestPos))
                                {
                                    cost[bestPos.X + x, bestPos.Y + y] = newCost;
                                    link[bestPos.X + x, bestPos.Y + y].X = bestPos.X;
                                    link[bestPos.X + x, bestPos.Y + y].Y = bestPos.Y;
                                    //Debug.WriteLine("X: " + bestPos.X + " Y: " + bestPos.Y);
                                }
                            }
                        //}
                        //catch
                        //{
                            //Debug.WriteLine("Here");
                        //}*/

                    }
                }
                if (bestPos == plr.GridPosition)
                {
                    done = true;
                    //Debug.WriteLine("DONE");
                }
            }

            done = false;
            Coord2 nextClosed = plr.GridPosition;
            while (!done)
            {
                inPath[nextClosed.X, nextClosed.Y] = true;

                nextClosed = link[nextClosed.X, nextClosed.Y];

                if (nextClosed == bot.GridPosition)
                    done = true;
            }

            DateTime end = DateTime.Now;
            TimeSpan duration = end - start;
            return duration.Milliseconds;

        }
    }
}
