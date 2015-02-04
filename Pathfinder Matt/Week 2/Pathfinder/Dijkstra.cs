using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Pathfinder
{
    class Dijkstra
    {
        public bool[,] closed; // Whether or not location is closed
        public float[,] cost; // Cost value for each location
        public Coord2[,] link; // Link for each location = coords of a neighbouring location
        public bool[,] inPath; // Whether or not a location is in the final path

        public Dijkstra()
        {
            closed = new bool[40, 40];
            cost = new float[40, 40];
            link = new Coord2[40, 40];
            inPath = new bool[40, 40];
        }

        public void Build(Level level, AiBotBase bot, Player plr)
        {
            for (int i = 0; i < 40; i++)
            {
                for (int ii = 0; ii < 40; ii++)
                {
                    closed[i, ii] = false;
                    cost[i, ii] = 1000000;
                    link[i, ii] = new Coord2(-1, -1);
                    inPath[i, ii] = false;
                }
            }

            cost[bot.GridPosition.X, bot.GridPosition.Y] = 0;

            bool repeat = true;

            while (repeat)
            {
                Coord2 holdC = new Coord2(0, 0);
                Coord2 holdCM;
                float holdI = 10000000;

                for (int i = 0; i < 40; i++)
                {
                    for (int ii = 0; ii < 40; ii++)
                    {
                        if (cost[i, ii] < holdI && closed[i, ii] != true)
                        {
                            holdI = cost[i, ii];
                            holdC.X = i;
                            holdC.Y = ii;
                        }
                    }
                }

                closed[holdC.X, holdC.Y] = true;

                holdCM.X = holdC.X - 1; // Up
                holdCM.Y = holdC.Y;
                if (level.ValidPosition(holdCM))
                {
                    if (cost[holdCM.X, holdCM.Y] > cost[holdC.X, holdC.Y] + 1)
                    {
                        cost[holdCM.X, holdCM.Y] = cost[holdC.X, holdC.Y] + 1;
                        link[holdCM.X, holdCM.Y].X = holdC.X;
                        link[holdCM.X, holdCM.Y].Y = holdC.Y;
                    }
                }

                holdCM.X = holdC.X - 1; // Up-Right
                holdCM.Y = holdC.Y + 1;
                if (level.ValidPosition(holdCM))
                {
                    if (cost[holdCM.X, holdCM.Y] > cost[holdC.X, holdC.Y] + 1.4)
                    {
                        cost[holdCM.X, holdCM.Y] = cost[holdC.X, holdC.Y] + 1.4f;
                        link[holdCM.X, holdCM.Y].X = holdC.X;
                        link[holdCM.X, holdCM.Y].Y = holdC.Y;
                    }
                }

                holdCM.X = holdC.X; // Right
                holdCM.Y = holdC.Y + 1;
                if (level.ValidPosition(holdCM))
                {
                    if (cost[holdCM.X, holdCM.Y] > cost[holdC.X, holdC.Y] + 1)
                    {
                        cost[holdCM.X, holdCM.Y] = cost[holdC.X, holdC.Y] + 1;
                        link[holdCM.X, holdCM.Y].X = holdC.X;
                        link[holdCM.X, holdCM.Y].Y = holdC.Y;
                    }
                }

                holdCM.X = holdC.X + 1; // Down-Right
                holdCM.Y = holdC.Y + 1;
                if (level.ValidPosition(holdCM))
                {
                    if (cost[holdCM.X, holdCM.Y] > cost[holdC.X, holdC.Y] + 1.4)
                    {
                        cost[holdCM.X, holdCM.Y] = cost[holdC.X, holdC.Y] + 1.4f;
                        link[holdCM.X, holdCM.Y].X = holdC.X;
                        link[holdCM.X, holdCM.Y].Y = holdC.Y;
                    }
                }

                holdCM.X = holdC.X + 1; // Down
                holdCM.Y = holdC.Y;
                if (level.ValidPosition(holdCM))
                {
                    if (cost[holdCM.X, holdCM.Y] > cost[holdC.X, holdC.Y] + 1)
                    {
                        cost[holdCM.X, holdCM.Y] = cost[holdC.X, holdC.Y] + 1;
                        link[holdCM.X, holdCM.Y].X = holdC.X;
                        link[holdCM.X, holdCM.Y].Y = holdC.Y;
                    }
                }

                holdCM.X = holdC.X + 1; // Down-Left
                holdCM.Y = holdC.Y - 1;
                if (level.ValidPosition(holdCM))
                {
                    if (cost[holdCM.X, holdCM.Y] > cost[holdC.X, holdC.Y] + 1.4)
                    {
                        cost[holdCM.X, holdCM.Y] = cost[holdC.X, holdC.Y] + 1.4f;
                        link[holdCM.X, holdCM.Y].X = holdC.X;
                        link[holdCM.X, holdCM.Y].Y = holdC.Y;
                    }
                }

                holdCM.X = holdC.X; // Left
                holdCM.Y = holdC.Y - 1;
                if (level.ValidPosition(holdCM))
                {
                    if (cost[holdCM.X, holdCM.Y] > cost[holdC.X, holdC.Y] + 1)
                    {
                        cost[holdCM.X, holdCM.Y] = cost[holdC.X, holdC.Y] + 1;
                        link[holdCM.X, holdCM.Y].X = holdC.X;
                        link[holdCM.X, holdCM.Y].Y = holdC.Y;
                    }
                }

                holdCM.X = holdC.X - 1; // Up-Left
                holdCM.Y = holdC.Y - 1;
                if (level.ValidPosition(holdCM))
                {
                    if (cost[holdCM.X, holdCM.Y] > cost[holdC.X, holdC.Y] + 1.4)
                    {
                        cost[holdCM.X, holdCM.Y] = cost[holdC.X, holdC.Y] + 1.4f;
                        link[holdCM.X, holdCM.Y].X = holdC.X;
                        link[holdCM.X, holdCM.Y].Y = holdC.Y;
                    }
                }

                if (holdC.X == plr.GridPosition.X && holdC.Y == plr.GridPosition.Y)
                {
                    repeat = false;
                }
            }

            repeat = true;
            Coord2 nextClosed = plr.GridPosition; // Start of path

            while (repeat)
            {
                inPath[nextClosed.X, nextClosed.Y] = true;
                nextClosed = link[nextClosed.X, nextClosed.Y];
                if (nextClosed == bot.GridPosition)
                    repeat = false;
            }

            
        }
    }
}
