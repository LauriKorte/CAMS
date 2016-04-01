using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public static class MazeGenerator
    {
        public static void Generate(TileMap tm, int interconnectedness, int openSpots)
        {
            /*
                We generate an x by x maze,
                in a grid where walls take 1 cell.
                The whole maze is bordered by 1 cell too.
            */
            Vector2 size = tm.size;

            //In case someone wants a really, really, really open maze
            if (interconnectedness <= 1) 
                interconnectedness = 1; //We just give a really open maze

            List<List<bool>> maze = new List<List<bool>>(); // Array containing cell information
            for (int i = 0; i < size.x; i++)
            {
                List<bool> mcol = new List<bool>();
                for (int j = 0; j < size.y; j++)
                    mcol.Add(false);
                maze.Add(mcol);
            }
            Random random = new Random();

            //A recursive algorithm to emulate walk of a drunk
            //because that's the best way to generate mazes

            Action<Vector2> drunkenWalk = null;
            drunkenWalk = (Vector2 pos) =>
            {
                //Set current position to open
                maze[pos.x][pos.y] = true;

                //Array of four cardinal directions
                int[] directions = new int[4];
                //0 = Y-, 1 = X+, 2 = Y+, 3 = X-
                for (int i = 0; i < 4; i++)
                    directions[i] = i;
                

                //Randomize possible directions
                int[] arr = directions.OrderBy(x => random.Next()).ToArray();


                //Walk to every direction (in random order)
                for (int i = 0; i < 4; i++)
                {
                    Vector2 target = new Vector2(0,0);
                    switch (arr[i])
                    {
                        case 0:
                            target.y -= 1;
                            break;
                        case 1:
                            target.x += 1;
                            break;
                        case 2:
                            target.y += 1;
                            break;
                        case 3:
                        default:
                            target.x -= 1;
                            break;
                    }

                    //The wall between current position and walk target
                    Vector2 wall = pos + target;
                    //Walk target
                    Vector2 jump = pos + target + target;

                    //If the target is out of bounds
                    //try the next direction
                    if (jump.x < 1 || jump.y < 1)
                        continue;
                    if (jump.x >= size.x - 1 || jump.y >= size.y - 1)
                        continue;

                    //If the target is already walked once
                    if (maze[jump.x][jump.y] == true)
                    {
                        //We open the way there anyway, if dice rolls right
                        if (random.Next(interconnectedness) == 0)
                            maze[wall.x][wall.y] = true;

                        //then try the next direction
                        continue;
                    }

                    //If all's good, open the way to there
                    //and continue the walk
                    maze[wall.x][wall.y] = true;       
                    drunkenWalk(jump);

                    //then try the next direction
                }
            };

            //Start the walk from the cell (1,1)
            drunkenWalk(new Vector2(1, 1));

            //After generating a maze, spice thing up a bit
            for (int i = 0; i < openSpots; i++)
            {
                Vector2 rpos;
                rpos.x = 1 + random.Next(size.x - 2);
                rpos.y = 1 + random.Next(size.y - 2);

                //by randomly opening spots in the maze
                maze[rpos.x][rpos.y] = true;
            }

            //Then write the maze to the TileMap
            for (int i = 0; i < size.x; i++)
                for (int j = 0; j < size.y; j++)
                {
                    Tile t = tm.getTile(new Vector2(i, j));
                    //TODO maze tile types
                    if (maze[i][j])
                        t.type = 1;
                    else
                        t.type = 0;
                }
            SpreadRandomRooms(tm, 6, 3, 6);
        }

        public static void SpreadRandomRooms(TileMap tm, int count, int minSize, int maxSize)
        {
            Vector2 size = tm.size;
            size.x -= 2;
            size.y -= 2;
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                Vector2 soize;
                soize.x = r.Next(maxSize-minSize)+minSize;
                soize.y = r.Next(maxSize-minSize)+minSize;
                Vector2 mpos;
                mpos.x = r.Next(size.x)+1;
                mpos.y = r.Next(size.y)+1;

                Vector2 add;
                add = mpos + soize;
                if (add.x >= tm.size.x)
                    add.x = tm.size.x - 1;
                if (add.y >= tm.size.y)
                    add.y = tm.size.y - 1;

                for (int x = mpos.x; x < add.x; x++)
                    for (int y = mpos.y; y < add.y; y++)
                    {
                        Tile t = tm.getTile(new Vector2(x, y));
                        t.type = 1;
                    }
            }
        }
    }
}
