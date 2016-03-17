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

        public GameLogic()
        {
            
            actors = new List<Actor>();
            tileMap = new TileMap();
            tileMap.initialize(new Vector2(16, 16));

            tileMap.getTile(new Vector2(2, 2)).type = 1;
            player = new Actor();
            player.image = "ac_cougar";
            player.depth = 1;
            player.position.x = 4;
            player.position.y = 4;

            actors.Add(player);
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
        }
    }
}
