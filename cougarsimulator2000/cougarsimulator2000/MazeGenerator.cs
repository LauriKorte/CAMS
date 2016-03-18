using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public class MazeGenerator
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
                for (int j = 0; j < size.x; j++)
                    mcol.Add(false);
                maze.Add(mcol);
            }
            Random random = new Random();

            Action<Vector2> drunkenWalk = null;
            drunkenWalk = (Vector2 pos) =>
            {
                maze[pos.x][pos.y] = true;

                //0 = Y-, 1 = X+, 2 = Y+, 3 = X-
                int[] directions = new int[4];
                for (int i = 0; i < 4; i++)
                    directions[i] = i;
                

                //Randomize possible directions
                int[] arr = directions.OrderBy(x => random.Next()).ToArray();
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
                    if (jump.x < 1 || jump.y < 1)
                        continue;
                    if (jump.x >= size.x - 1 || jump.y >= size.y - 1)
                        continue;
                    if (maze[jump.x][jump.y] == true)
                    {
                        if (random.Next(interconnectedness) == 0)
                            maze[wall.x][wall.y] = true;
                        else
                            continue;
                    }
                    maze[wall.x][wall.y] = true;       
                    drunkenWalk(jump);
                }
            };
            drunkenWalk(new Vector2(1, 1));
            for (int i = 0; i < openSpots; i++)
            {
                Vector2 rpos;
                rpos.x = 1 + random.Next(size.x - 2);
                rpos.y = 1 + random.Next(size.y - 2);
                maze[rpos.x][rpos.y] = true;
            }

            for (int i = 0; i < size.x; i++)
                for (int j = 0; j < size.x; j++)
                {
                    Tile t = tm.getTile(new Vector2(i, j));
                    if (maze[i][j])
                        t.type = 1;
                    else
                        t.type = 0;
                }
        }
    }
}
