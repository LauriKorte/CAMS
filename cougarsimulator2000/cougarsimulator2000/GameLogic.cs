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
            tileMap.initialize(new Vector2(16, 16));
            for (int i = 1; i < 15; i++)
                for (int j = 1; j < 15; j++)
                    tileMap.getTile(new Vector2(i,j)).type = 1;
            player = new Actor();
            player.image = "ac_tex";
            player.depth = 2;
            player.position.x = 4;
            player.position.y = 4;

            actors.Add(player);

            Random r = new Random();
            for (int i = 0; i < 16; i++)
            {
                Actor a = new Actor();
                a.image = "ac_cougar";
                a.depth = 1;
                a.position.x = r.Next(16);
                a.position.y = r.Next(16);
                cougars.Add(a);
                actors.Add(a);
            }
        }       

        private void updateActors()
        {
            Random r = new Random();
            foreach (var cugar in cougars)
            {
                cugar.position.x += r.Next(3) - 1;
                cugar.position.y += r.Next(3) - 1;
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

            player.position += off;
            updateActors();
        }
    }
}
