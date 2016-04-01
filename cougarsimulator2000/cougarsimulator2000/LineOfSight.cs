using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public static class LineOfSight
    {
        //Returns a list of edge cells on a circle
        //Why do we need this? I don't know really
        public static List<Vector2> GetCellsOnRadius(Vector2 center, double radius)
        {
            //The center in floating point
            double fcx = 0.5;
            double fcy = 0.5;

            List<Vector2> cells = new List<Vector2>();

            //Squared radius we use for comparison
            double rsqrd = radius * radius;

            for (int i = -(int)(radius + 1.0); i < (int)(radius + 1.0); i++)
                for (int j = -(int)(radius + 1.0); j < (int)(radius + 1.0); j++)
                {
                    //Get all the corner positions for the current cell

                    double c1x = i - fcx;
                    double c1y = j - fcy;

                    double c2x = i + 1 - fcx;
                    double c2y = j - fcy;

                    double c3x = i + 1 - fcx;
                    double c3y = j + 1 - fcy;

                    double c4x = i + 1 - fcx;
                    double c4y = j - fcy;

                    //If a one corner is in the circle and one isn't, we're at the edge
                    bool ins = false;
                    if ((c1x * c1x + c1y * c1y) < rsqrd)
                        ins = true;
                    bool secins = false;
                    if ((c2x * c2x + c2y * c2y) < rsqrd)
                        secins = true;
                    if (secins != ins)
                    {
                        cells.Add(new Vector2(i, j) + center);
                        continue;
                    }

                    secins = false;
                    if ((c3x * c3x + c3y * c3y) < rsqrd)
                        secins = true;
                    if (secins != ins)
                    {
                        cells.Add(new Vector2(i, j) + center);
                        continue;
                    }

                    secins = false;
                    if ((c4x * c4x + c4y * c4y) < rsqrd)
                        secins = true;
                    if (secins != ins)
                    {
                        cells.Add(new Vector2(i, j) + center);
                        continue;
                    }
                }

            return cells;
        }

        //Crude implementation to line of sight

        //IsBlockingCellDelegate is defined within PathFinding.cs
        //Returns true if line of sight exists between the cells
        public static bool GetLineOfSight(IsBlockingCellDelegate isBlocking, Vector2 from, Vector2 to, double targetDeltaX = 0.5, double targetDeltaY = 0.5)
        {
            //Let's check the first cell
            if (isBlocking(from))
                return false;

            //If the beginning and end are the same
            if (from == to)
            {
                //Line of sight exists
                return true;
            }

            //Special case:
            //When the line is perfectly vertical

            if (from.x == to.x)
            {
                int dif = to.y - from.y;
                Vector2 dir;

                if (dif < 0)
                {
                    dir = new Vector2(0, -1);
                    dif = -dif;
                }
                else
                {
                    dir = new Vector2(0, 1);
                }

                while (from != to)
                {
                    from += dir;
                    if (isBlocking(from))
                        return false;
                }
                return true;
            }
            //These represent position within the current cell
            //If we we're to allow sub-cell accuracy for our line of sight,
            //We would provide arguments to modify these
            double yDelta = 0.5;
            double xDelta = 0.5;

            double yDif = to.y - from.y;
            double xDif = to.x - from.x;

            xDif -= xDelta - targetDeltaX;
            yDif -= yDelta - targetDeltaY;

            //We make the algorithm work in all four quadrants
            //by couple of checks

            Vector2 xdir = new Vector2(1, 0);
            Vector2 ydir = new Vector2(0, 1);

            if (xDif < 0)
            {
                xDif = -xDif;
                xdir = new Vector2(-1, 0);
            }
            else
                xDelta = 1 - xDelta;

            if (yDif < 0)
            {
                yDif = -yDif;
                ydir = new Vector2(0, -1);

            }
            else
                yDelta = 1 - yDelta;

            //If we didn't check for the special case up there
            //We'd end up with division by zero here, which wouldn't work out
            //for our algorithm
            double curv = yDif / xDif;

            //If our line is perfectly horizontal, we get a division by zero here
            //but it's okay
            //if yDif = 0, then the above "curv" is 0 too, and we'll never
            //end up using the "icurv" variable here in the algorithm
            double icurv = xDif / yDif;

            //The distance between target cells:

            //We can use the "manhattan" distance between cells, because
            //we move from cell to cell only in cardinal directions

            int dst = Math.Abs(from.x - to.x) + Math.Abs(from.y - to.y);

            //The end condition for our raycasting line of sight algorithm
            //should be situation when (currentCell == lastCell), but floating point may
            //be funky and we don't want to end up in an infinite loop

            //Our calculations may "miss" the last cell, if we're speaking of very, very long
            //lines, or very large numbers. 

            //That's why we calculate the count of iterated cells, instead of checking 
            //if we've reached the end

            while (dst > 0)
            {
                //The algorithm actually works by checking which
                //cell edge is reached first

                //If we were to continue to the next cell along the X axis
                //this is the amount the Y coordinate would increase
                double yAdd = xDelta * curv;

                //If we were to reach the horizontal edge of the cell first instead
                if (yAdd > yDelta)
                {
                    //We approach the vertical edge by this amount
                    xDelta -= yDelta * icurv;

                    //and move to the next horizontal edge
                    yDelta = 1.0;
                    from += ydir;
                }
                else
                {
                    //Else, we actually move to the next cell alogn X axis
                    yDelta -= yAdd;

                    xDelta = 1.0;
                    from += xdir;
                }
                //Now, check this cell
                if (isBlocking(from))
                    return false;
                dst--;
            }

            return true;
        }

        //A variation of the above, providing rounder corners
        //and the ability to peek through diagonal cracks
        public static bool GetDiamondLineOfSight(IsBlockingCellDelegate isBlocking, Vector2 from, Vector2 to, double targetDeltaX = 0.5, double targetDeltaY = 0.5)
        {
            //In this algorithm we rotate the world 45 degrees, so each wall is diamond shaped
            //In order to do this, we need to modify the coordinates a bit

            //This algorithm really could use a diagram for explanation
            //...so too bad

            //But here's the formula for the 'diamond space'
            //diamond_x = grid_x + grid_y
            //diamond_y = grid_y - grid_x

            //In this 'diamond space', every cell doesn't uniformly map to another in our
            //regular 'grid space'. We have these 'filler cells' that just sort of 
            //make the map more round

            //Couldn't be clearer, right?


            //We need a layer to transform the coordinates
            IsBlockingCellDelegate ibcd = (v)
                =>
            {
                //We transform from our diamond grid to
                //normal grid
                int sum = v.x + v.y;

                //Observation, if the sum of both components are odd
                //it's a filler cell
                if (sum % 2 == 1)
                {
                    //and those cells we just pass through
                    return false;
                }
                //Otherwise...
                Vector2 tcoord;

                //We transform from the diamond coordinates
                //to normal grid coordinates

                //According to the solution of this:
                //diamond_x = grid_x + grid_y
                //diamond_y = grid_y - grid_x

                //we do this
                tcoord.y = (v.y + v.x) / 2;
                tcoord.x = v.x - tcoord.y;

                return isBlocking(tcoord);
            };
            //Before we do anything,
            //We transform some coordinates

            Vector2 dfrom;
            Vector2 dto;

            dfrom.x = from.x + from.y;
            dfrom.y = from.y - from.x;

            dto.x = to.x + to.y;
            dto.y = to.y - to.x;

            //And call the regular thingymabob

            return GetLineOfSight(ibcd, dfrom, dto);
            //And it just werks
        }
    }
}
