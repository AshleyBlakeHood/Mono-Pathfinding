using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Pathfinder
{
    class AiBotStatic : AiBotBase
    {
        public AiBotStatic(int x, int y)
            : base(x, y)
        {

        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
        }
    }

    class AiBotPathFollower : AiBotBase
    {
        
        public AiBotPathFollower(int x, int y)
            : base(x, y)
        {
            pathToFollow = new List<Coord2>();
        }

        public void newPath(List<Coord2> newPathCoords)
        {
            pathToFollow = newPathCoords;
            pathIndex = pathToFollow.Count - 1;
        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            try
            {
                SetNextGridPosition(pathToFollow[pathIndex], level);
                pathIndex--;
            }
            catch
            {

            }
        }
    }

    class AiBotPathfinderSimple : AiBotBase
    {
        public AiBotPathfinderSimple(int x, int y)
            : base(x, y)
        {

        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            Coord2 pos;
            pos = GridPosition;

            if (plr.GridPosition.X < GridPosition.X)
            {
                pos.X -= 1;
            }
            else if (plr.GridPosition.X > GridPosition.X)
            {
                pos.X += 1;
            }
            else if (plr.GridPosition.Y < GridPosition.Y)
            {
                pos.Y -= 1;
            }
            else if (plr.GridPosition.Y > GridPosition.Y)
            {
                pos.Y += 1;
            }

            SetNextGridPosition(pos, level);
        }
    }

    class AiBotPathfinderSimple2 : AiBotBase
    {
        int movementDir = 0;
        public AiBotPathfinderSimple2(int x, int y, Coord2 pPos) : base(x,y)
        {
            if (pPos.X < GridPosition.X)
            {
                movementDir = 1;
            }
            else if (pPos.X > GridPosition.X)
            {
                movementDir = 2;
            }
            else if (pPos.Y < GridPosition.Y)
            {
                movementDir = 3;
            }
            else if (pPos.Y > GridPosition.Y)
            {
                movementDir = 4;
            }
        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            Coord2 pos;
            pos = GridPosition;

            if(level.ValidPosition(SetNextPos(movementDir, pos)))
            {
                SetNextGridPosition(SetNextPos(movementDir, pos), level);
            }
            else
            {

            }
        }

        Coord2 SetNextPos(int movementDirection, Coord2 pos)
        {
            Coord2 temp = pos;
            switch (movementDirection)
            {
                case 1:
                    temp.X -= 1;
                    break;
                case 2:
                    temp.X += 1;
                    break;
                case 3:
                    temp.Y -= 1;
                    break;
                case 4:
                    temp.Y += 1;
                    break;
                default:
                    break;
            }
            return temp;
        }
    }

    /*class Dijkstra
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
                Coord2 bestPos = new Coord2(0, 0);
                Coord2 checkPos;
                float checkCost = 10000000;

                for (int i = 0; i < 40; i++)
                {
                    for (int ii = 0; ii < 40; ii++)
                    {
                        if (cost[i, ii] < checkCost && closed[i, ii] != true)
                        {
                            checkCost = cost[i, ii];
                            bestPos.X = i;
                            bestPos.Y = ii;
                        }
                    }
                }

                closed[bestPos.X, bestPos.Y] = true;

                checkPos.X = bestPos.X - 1; // Up
                checkPos.Y = bestPos.Y;
                if (level.ValidPosition(checkPos))
                {
                    if (cost[checkPos.X, checkPos.Y] > cost[bestPos.X, bestPos.Y] + 1)
                    {
                        cost[checkPos.X, checkPos.Y] = cost[bestPos.X, bestPos.Y] + 1;
                        link[checkPos.X, checkPos.Y].X = bestPos.X;
                        link[checkPos.X, checkPos.Y].Y = bestPos.Y;
                    }
                }

                checkPos.X = bestPos.X - 1; // Up-Right
                checkPos.Y = bestPos.Y + 1;
                if (level.ValidPosition(checkPos))
                {
                    if (cost[checkPos.X, checkPos.Y] > cost[bestPos.X, bestPos.Y] + 1.4)
                    {
                        cost[checkPos.X, checkPos.Y] = cost[bestPos.X, bestPos.Y] + 1.4f;
                        link[checkPos.X, checkPos.Y].X = bestPos.X;
                        link[checkPos.X, checkPos.Y].Y = bestPos.Y;
                    }
                }

                checkPos.X = bestPos.X; // Right
                checkPos.Y = bestPos.Y + 1;
                if (level.ValidPosition(checkPos))
                {
                    if (cost[checkPos.X, checkPos.Y] > cost[bestPos.X, bestPos.Y] + 1)
                    {
                        cost[checkPos.X, checkPos.Y] = cost[bestPos.X, bestPos.Y] + 1;
                        link[checkPos.X, checkPos.Y].X = bestPos.X;
                        link[checkPos.X, checkPos.Y].Y = bestPos.Y;
                    }
                }

                checkPos.X = bestPos.X + 1; // Down-Right
                checkPos.Y = bestPos.Y + 1;
                if (level.ValidPosition(checkPos))
                {
                    if (cost[checkPos.X, checkPos.Y] > cost[bestPos.X, bestPos.Y] + 1.4)
                    {
                        cost[checkPos.X, checkPos.Y] = cost[bestPos.X, bestPos.Y] + 1.4f;
                        link[checkPos.X, checkPos.Y].X = bestPos.X;
                        link[checkPos.X, checkPos.Y].Y = bestPos.Y;
                    }
                }

                checkPos.X = bestPos.X + 1; // Down
                checkPos.Y = bestPos.Y;
                if (level.ValidPosition(checkPos))
                {
                    if (cost[checkPos.X, checkPos.Y] > cost[bestPos.X, bestPos.Y] + 1)
                    {
                        cost[checkPos.X, checkPos.Y] = cost[bestPos.X, bestPos.Y] + 1;
                        link[checkPos.X, checkPos.Y].X = bestPos.X;
                        link[checkPos.X, checkPos.Y].Y = bestPos.Y;
                    }
                }

                checkPos.X = bestPos.X + 1; // Down-Left
                checkPos.Y = bestPos.Y - 1;
                if (level.ValidPosition(checkPos))
                {
                    if (cost[checkPos.X, checkPos.Y] > cost[bestPos.X, bestPos.Y] + 1.4)
                    {
                        cost[checkPos.X, checkPos.Y] = cost[bestPos.X, bestPos.Y] + 1.4f;
                        link[checkPos.X, checkPos.Y].X = bestPos.X;
                        link[checkPos.X, checkPos.Y].Y = bestPos.Y;
                    }
                }

                checkPos.X = bestPos.X; // Left
                checkPos.Y = bestPos.Y - 1;
                if (level.ValidPosition(checkPos))
                {
                    if (cost[checkPos.X, checkPos.Y] > cost[bestPos.X, bestPos.Y] + 1)
                    {
                        cost[checkPos.X, checkPos.Y] = cost[bestPos.X, bestPos.Y] + 1;
                        link[checkPos.X, checkPos.Y].X = bestPos.X;
                        link[checkPos.X, checkPos.Y].Y = bestPos.Y;
                    }
                }

                checkPos.X = bestPos.X - 1; // Up-Left
                checkPos.Y = bestPos.Y - 1;
                if (level.ValidPosition(checkPos))
                {
                    if (cost[checkPos.X, checkPos.Y] > cost[bestPos.X, bestPos.Y] + 1.4)
                    {
                        cost[checkPos.X, checkPos.Y] = cost[bestPos.X, bestPos.Y] + 1.4f;
                        link[checkPos.X, checkPos.Y].X = bestPos.X;
                        link[checkPos.X, checkPos.Y].Y = bestPos.Y;
                    }
                }

                if (bestPos.X == plr.GridPosition.X && bestPos.Y == plr.GridPosition.Y)
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
    }*/

    
}
