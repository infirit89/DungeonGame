using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace DungeonGame
{
    class Enemy : Entity
    {
        public Enemy(Vector2 position)
            : base(10, 10, 'x', position) 
        {

        }
    }
}
