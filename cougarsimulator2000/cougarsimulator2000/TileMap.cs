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
    }


    public class Tile
    {
        public int type = 0;
    }

    public class TileMap
    {
        private int width = 16;
        private int height = 16;
        private List<Tile> tiles;

        public Vector2 getSize()
        {
            return new Vector2(width, height);
        }

        public void initialize(Vector2 size)
        {
            width = size.x;
            height = size.y;
            tiles = new List<Tile>();
            
            for (int i = 0; i < width; i++)
                for (int j = 0; j < width; j++)
                    tiles.Add(new Tile());
        }

        public Tile getTile(Vector2 pos)
        {
            int vpos = pos.x + pos.y * width;
            if (vpos < 0 || vpos >= tiles.Count)
                throw new IndexOutOfRangeException();
            return tiles[vpos];
        }

    }
}
