using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public enum Input
    {
        North = 1,
        South = 2,
        East = 4,
        West = 8,
        NorthEast = North|East,
        NorthWest = North|West,
        SouthEast = South|East,
        SouthWest = South|West
    }

    public class GameLogic
    {
        public Actor player;

        public TileMap tileMap
        {
            get;
        }

        public List<Actor> actors
        {
            get;
        }

        private List<Actor> cougars = new List<Actor>();

        public GameLogic()
        {
            
            actors = new List<Actor>();
            tileMap = new TileMap();

            //Make sure the map size is uneven
            tileMap.initialize(new Vector2(17, 17));

            //otherwise the maze generator makes a mess
            MazeGenerator.Generate(tileMap,25,25);

            /*
            //Fill with empty prairie
            for (int i = 1; i < 15; i++)
                for (int j = 1; j < 15; j++)
                    tileMap.getTile(new Vector2(i,j)).type = 1;
            */
            player = new Actor();
            player.image = "ac_tex";
            player.depth = 2;
            player.position.x = 1;
            player.position.y = 1;

            actors.Add(player);

            Random r = new Random();
            for (int i = 0; i < 16; i++)
            {
                Actor a = new Actor();
                a.image = "ac_cougar";
                a.depth = 1;
                a.position.x = r.Next(14)+1;
                a.position.y = r.Next(14)+1;
                cougars.Add(a);
                actors.Add(a);
            }
        }       

        private void updateActors()
        {
            Random r = new Random();
            foreach (var cugar in cougars)
            {
                //Cougars area chaotic creatures
                Vector2 trypos = cugar.position;
                trypos.x += r.Next(3) - 1;
                trypos.y += r.Next(3) - 1;

                if (!isTileBlocking(trypos))
                    cugar.position = trypos;
            }
        }

        public void enterInput(Input input)
        {
            int i = (int)input;
            Vector2 off = new Vector2(0,0);
            if ((i & (int)Input.East) != 0)
                off.x++;

            if ((i & (int)Input.West) != 0)
                off.x--;

            if ((i & (int)Input.North) != 0)
                off.y--;

            if ((i & (int)Input.South) != 0)
                off.y++;

            Vector2 trypos = player.position + off;
            if (!isTileBlocking(trypos))
                player.position += off;
            updateActors();
        }

        private bool isTileBlocking(Vector2 trypos)
        {
            if (trypos.x < 0 || trypos.y < 0 || trypos.x >= tileMap.size.x || trypos.y >= tileMap.size.y)
                return true;
            if (tileMap.getTile(trypos).type == 1)
                return false;
            return true;
        }
    }
}
