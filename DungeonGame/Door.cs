using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace DungeonGame
{
    class Door : Tile
    {
        public Door(Vector2 position)
            : base(position, '^') 
        {
        }
    }
}
