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
}
