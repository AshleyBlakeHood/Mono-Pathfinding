using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinder
{
    class AiBotSimple: AiBotBase
    {
        public AiBotSimple(int x, int y): base(x,y)
        {

        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            Coord2 newPos;
            newPos = GridPosition;

            if (plr.GridPosition.X > GridPosition.X)
            {
                newPos.X += 1;
            }
            else
            {
                if (plr.GridPosition.X < GridPosition.X)
                {
                    newPos.X -= 1;
                }
                else
                {
                    if (plr.GridPosition.Y > GridPosition.Y)
                    {
                        newPos.Y += 1;
                    }
                    else
                    {
                        if (plr.GridPosition.Y < GridPosition.Y)
                        {
                            newPos.Y -= 1;
                        }
                    }
                }
            }

            SetNextGridPosition(newPos, level);
        }
    }
}
