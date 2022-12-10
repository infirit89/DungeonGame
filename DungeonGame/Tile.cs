using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace DungeonGame
{
    class Tile
    {
        public Tile(Vector2 position, char icon) 
        {
            Position = position;
            Icon = icon;
        }

        public Vector2 Position;
        public char Icon;
    }
}
