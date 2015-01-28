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

            /*if(level.ValidPosition(pos))
            {
                SetNextGridPosition(pos, level);
            }
            else
            {

            }*/
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

    class Dijkstra
    {
        public bool[,] closed; //whether or not location is closed
        public float[,] cost; //cost values for each location
        public Coord2[,] link; //link for each location == coords of neighbouring location
        public bool[,] inPath; //whether or not a location is in the final path

        float[,] costings = new float[3, 3] {{1.4f, 1.0f, 1.4f}, {1.0f, 0.0f, 1.0f}, {1.4f, 1.0f, 1.4f}};

        public Dijkstra()
        {
            closed = new bool[40, 40];
            cost = new float[40, 40];
            link = new Coord2[40, 40];
            inPath = new bool[40, 40];   
        }

        public void Build(Level level, AiBotBase bot, Player plr)
        {
            for(int y = 0; y < 40; y++)
            {
                for(int x = 0; x < 40; x++)
                {
                    closed[x, y] = false;
                    cost[x, y] = 1000000.0f;
                    link[x, y] = new Coord2(-1, -1);
                    inPath[x, y] = false;
                }
            }

            cost[bot.GridPosition.X, bot.GridPosition.Y] = 0.0f;
            bool done = false;

            while(!done)
            {
                Coord2 bestPos = new Coord2(0,0);

                for (int y = 0; y < 40; y++)
                {
                    for (int x = 0; x < 40; x++)
                    {
                        if(cost[x,y] < cost[bestPos.X, bestPos.Y] && !closed[x,y])
                        {
                            bestPos.X = x;
                            bestPos.Y = y;
                        }
                    }
                }

                closed[bestPos.X, bestPos.Y] = true;

                if (bestPos == plr.GridPosition)
                {
                    done = true;
                }

                for(int y = -1; y <= 1; y++)
                {
                    for(int x = -1; x <= 1; x++)
                    {
                        float costMod = costings[x + 1, y + 1];
                        float newCost = cost[bestPos.X, bestPos.Y] + costMod;
                        try
                        {
                            if (cost[bestPos.X + x, bestPos.Y + y] > newCost && level.ValidPosition(bestPos))
                            {
                                cost[bestPos.X + x, bestPos.Y + y] = newCost;
                                link[bestPos.X + x, bestPos.Y + y] = bestPos;
                            }
                        }
                        catch
                        {

                        }  
                    }
                }

                
             
            }

            done = false;
            Coord2 nextClosed = plr.GridPosition;
            while(!done)
            {
                inPath[nextClosed.X, nextClosed.Y] = true;
                nextClosed = link[nextClosed.X, nextClosed.Y];
                if (nextClosed == bot.GridPosition) done = true;
            }

        }
    }
}
