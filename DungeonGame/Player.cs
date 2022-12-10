using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace DungeonGame
{
    class Player : Entity
    {
        public Player(int health, int damage, Vector2 position) 
            : base(health, damage, m_Icon, position)
        {
        }

        public Player(int health, int damage)
            : this(health, damage, new Vector2(0, 0)) 
        { }

        private const char m_Icon = '@';
    }
}
