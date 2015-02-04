using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinder
{
    class AiBotSimple2 : AiBotBase
    {
        bool canMoveRight = true;

        public AiBotSimple2(int x, int y)
            : base(x, y)
        {

        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            Coord2 newPos;
            Coord2 halfPos;
            newPos = GridPosition;
            halfPos = GridPosition;
            

            if (plr.GridPosition.X > GridPosition.X)
            {
                halfPos.X += 1;

                if (level.ValidPosition(halfPos) && canMoveRight == true)
                {
                    newPos.X += 1;
                }
                else
                {
                    halfPos.X -= 1;
                    halfPos.Y -= 1;
                    canMoveRight = false;

                    if (level.ValidPosition(halfPos))
                    {
                        newPos.Y -= 1;
                    }
                    else
                    {
                        newPos.X -= 1;

                        halfPos.X += 2;
                        if ((level.ValidPosition(halfPos)) && (!level.ValidPosition(newPos)))
                        {
                            canMoveRight = true;
                        }
                    }
                }
            }
            else
            {
                if (plr.GridPosition.X < GridPosition.X)
                {
                    halfPos.X -= 1;

                    if (level.ValidPosition(halfPos))
                    {
                        //newPos = halfPos;
                    }
                    else
                    {
                        //newPos.Y += 1;
                    }
                }
                else
                {
                    if (plr.GridPosition.Y > GridPosition.Y)
                    {
                        halfPos.Y += 1;

                        if (level.ValidPosition(halfPos))
                        {
                            newPos = halfPos;
                        }
                        else
                        {
                           // newPos.Y -= 1;
                        }
                    }
                    else
                    {
                        if (plr.GridPosition.Y < GridPosition.Y)
                        {
                            halfPos.Y -= 1;

                            if (level.ValidPosition(halfPos))
                            {
                                newPos = halfPos;
                            }
                            else
                            {
                               // newPos.Y += 1;
                            }
                        }
                    }
                }
            }

            
            

            SetNextGridPosition(newPos, level);
        }
    }
}
