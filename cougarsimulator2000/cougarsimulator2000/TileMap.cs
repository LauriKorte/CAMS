using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public struct Vector2
    {
        public int x;
        public int y;
        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static Vector2 operator +(Vector2 c1, Vector2 c2)
        {
            return new Vector2(c1.x + c2.x, c1.y + c2.y);
        }

        public static Vector2 operator -(Vector2 c1, Vector2 c2)
        {
            return new Vector2(c1.x - c2.x, c1.y - c2.y);
        }

        public static bool operator == (Vector2 c1, Vector2 c2)
        {
            return (c1.x == c2.x) && (c1.y == c2.y);
        }
        public static bool operator !=(Vector2 c1, Vector2 c2)
        {
            return !(c1 == c2);
        }

    }


    public class Tile
    {
        public int type = 0;
    }

    public class TileMap
    {
        public Vector2 size
        {
            get;
            private set;
        }
        private List<Tile> tiles;
        

        public void initialize(Vector2 size)
        {
            this.size = size;
            tiles = new List<Tile>();
            
            for (int i = 0; i < size.x; i++)
                for (int j = 0; j < size.y; j++)
                    tiles.Add(new Tile());
        }

        public Tile getTile(Vector2 pos)
        {
            int vpos = pos.x + pos.y * size.x;
            if (vpos < 0 || vpos >= tiles.Count)
                throw new IndexOutOfRangeException();
            return tiles[vpos];
        }

    }
}
