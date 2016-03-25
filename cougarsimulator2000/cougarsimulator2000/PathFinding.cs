using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public class PathFindResult
    {
        //If a path was found
        public bool foundPath = false;

        //List of steps to get from the beginning to the end
        //The first element is always the beginning, and the last
        //element is always the end
        public List<Vector2> steps = new List<Vector2>();
    }

    public delegate bool IsBlockingCellDelegate(Vector2 position);

    public static class PathFinding
    {
        private class PathFindCell
        {
            public Vector2 position;
            public PathFindCell parent;
            public PathFindCell(Vector2 pos, PathFindCell par)
            {
                position = pos;
                parent = par;
            }
        }

        //Function to find a path in a rectangular grid
        public static PathFindResult GetPath(IsBlockingCellDelegate isBlocking, Vector2 from, Vector2 to)
        {
            //We're supposed to find a path...

            PathFindResult result = new PathFindResult();
            result.foundPath = false;

            //Let's start with a couple of special cases

            //Either the beginning or the end of the path is in stone
            //--> No path found
            if (isBlocking(from) || isBlocking(to))
                return result;
            
            //List where we store the current "boundaries" of the search
            List<PathFindCell> activeCells = new List<PathFindCell>();

            //Set where we store the visited cells
            HashSet<Vector2> closedCells = new HashSet<Vector2>();

            //Add beginning to the active set (with parent set as null)
            activeCells.Add(new PathFindCell(from, null));

            //Also close the beginning, so we wont visit it twice
            closedCells.Add(from);

            PathFindCell foundPath = null;

            //Go till we run out of cells
            //... or we find the goal
            while (activeCells.Count > 0)
            {
                //We take a copy of the activeCells
                var copyCells = activeCells.ToArray();
                activeCells.Clear();

                foreach (var v in copyCells)
                {
                    //Check if we already found our goal
                    if (v.position == to)
                    {
                        //Then break with our found path
                        foundPath = v;
                        break;
                    }
                    //Else...

                    //List of all 8 cell neighbors
                    //for the cell (0,0)

                    Vector2[] dirs = new Vector2[]
                    {
                        new Vector2(1,1),
                        new Vector2(1,0),
                        new Vector2(1,-1),
                        new Vector2(0,-1),
                        new Vector2(-1,-1),
                        new Vector2(-1,0),
                        new Vector2(-1,1),
                        new Vector2(0,1),
                    };

                    //We run through the list of possible neighbors
                    foreach (Vector2 offset in dirs)
                    {
                        //The position we are trying to move to
                        Vector2 testPos = v.position + offset;

                        //If the cell is already visited...
                        if (closedCells.Contains(testPos))
                        {
                            //...we don't visit it again
                            continue;
                        }

                        //If the cell is blocking...
                        if (isBlocking(testPos))
                        {
                            //...we don't go there
                            continue;
                        }

                        //Then we close it
                        closedCells.Add(testPos);

                        //And visit it
                        activeCells.Add(new PathFindCell(testPos, v));
                    }
                }
            }

            //If a path was found
            if (foundPath != null)
            {
                result.foundPath = true;
                //Let us go through the steps
                while (foundPath != null)
                {
                    result.steps.Add(foundPath.position);
                    //We set the beginning cell's parent to null
                    //If we reach that, we break
                    foundPath = foundPath.parent;
                }

                //After that, reverse the list so we start from the beginning
                result.steps.Reverse();
            }

            return result;
        }
    }
}
